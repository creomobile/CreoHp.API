using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Models.PhraseCollections;
using CreoHp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CreoHp.Services
{
    public sealed class PhrasesCollectorService : IPhrasesCollectorService
    {
        static readonly string[] ExcludesInUrl = new string[] { "google", "yandex", "mail.ru", "vk.ru", "ok.ru" };
        static readonly HashSet<char> ExcludeSymbols = new HashSet<char> { '{', '}', '/' };

        readonly AppDbContext _dbContext;
        HashSet<string> _badUrls;

        public PhrasesCollectorService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        async Task<IEnumerable<Uri>> GetPages(string keyPhrase)
        {
            const string resPrefix = "/url?q=";
            const string protocolPrefix = "http";

            var webClient = new WebClient();
            var html = await webClient.DownloadStringTaskAsync($"https://www.google.com/search?q=\"{keyPhrase}\"");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var result = document
                .QuerySelectorAll("a")
                .Select(p => p.GetAttribute("href"))
                .Where(p => p.StartsWith(resPrefix + protocolPrefix))
                .Select(p => new Uri(p.Substring(resPrefix.Length, p.IndexOf("&") - resPrefix.Length)))
                .Where(p => ExcludesInUrl.All(e => !p.Host.Contains(e)));

            if (result.Any())
            {
                _badUrls ??= _dbContext.BadCollectionUrls.Select(p => p.InUrl).ToHashSet();
                result = result.Where(p => _badUrls.All(url => !p.Host.Contains(url, StringComparison.OrdinalIgnoreCase)));
            }

            return result;
        }

        async Task<IEnumerable<string>> GetPhrases(string keyPhrase, Uri page)
        {
            string html = null;
            var result = Enumerable.Empty<string>();
            try
            {
                var webClient = new WebClient();
                html = await webClient.DownloadStringTaskAsync(page);
            }
            catch // ignore
            {
            }

            if (html != null)
            {
                var parser = new HtmlParser();
                var document = parser.ParseDocument(html);
                var text = document.All.SelectMany(p => p.ChildNodes.OfType<IText>()).FirstOrDefault(p => p.Text.Contains(keyPhrase))?.Text;
                if (text != null)
                    result = text.Split('.').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p) && ExcludeSymbols.All(s => !p.Contains(s)));
            }
            if (!result.Any())
            {
                var badUrl = page.Host;
                _dbContext.BadCollectionUrls.Add(new BadCollectionUrl { InUrl = badUrl });
                if (_badUrls != null) _badUrls.Add(badUrl);
            }

            return result;
        }

        async Task<IEnumerable<string>> GetPhrases(PhraseCollection collection)
        {
            var result = Enumerable.Empty<string>();
            collection.SearchCount++;
            var keyPhrase = collection.OriginalText;
            var pages = await GetPages(keyPhrase);
            if (pages.Any())
            {
                var tasks = pages.Take(1).Select(p => GetPhrases(keyPhrase, p));
                await Task.WhenAll(tasks);
                result = tasks.SelectMany(p => p.Result);
            }
            if (result.Any())
            {
                var phraseFlags = result.Select(p => new
                {
                    Phrase = p,
                    AddedTask = _dbContext.PhraseCollections.AnyAsync(c => c.OriginalText.Contains(p) || p.Contains(c.OriginalText))
                });
                foreach (var flag in phraseFlags) await flag.AddedTask;
                result = phraseFlags.Where(p => !p.AddedTask.Result).Select(p => p.Phrase).ToArray();
            }

            if (!result.Any()) collection.CanSearch = false;

            return result;
        }

        async Task<PhraseCollection> GetCollection(IEnumerable<Guid> except, string keyPhrase = null)
        {
            return string.IsNullOrWhiteSpace(keyPhrase)
                ? await _dbContext.PhraseCollections
                    .Where(p => p.CanSearch && !except.Contains(p.Id))
                    .OrderBy(p => p.SearchCount)
                    .FirstOrDefaultAsync()
                : (await _dbContext.PhraseCollections
                    .FirstOrDefaultAsync(p => p.CanSearch && p.OriginalText == keyPhrase))
                    ?? new PhraseCollection { OriginalText = keyPhrase };
        }

        public async Task<string[]> Collect(string keyPhrase, int max)
        {
            if (max <= 0) throw new AppException("Invalid 'max' value");

            var collection = await GetCollection(Enumerable.Empty<Guid>(), keyPhrase);
            if (collection == null) return new string[0];
            var searched = new HashSet<Guid>();

            var result = new HashSet<string>();

            do
            {
                searched.Add(collection.Id);
                var phrases = await GetPhrases(collection);
                result.UnionWith(phrases);
                await _dbContext.SaveChangesAsync();
            } while (searched.Count < 5 && result.Count < max && (collection = await GetCollection(searched)) != null);

            return result.Take(max).ToArray();
        }
    }
}
