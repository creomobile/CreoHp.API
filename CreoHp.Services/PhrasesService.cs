using AutoMapper;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using CreoHp.Models.Phrases;
using CreoHp.Models.Tags;
using CreoHp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CreoHp.Services
{
    public sealed class PhrasesService : IPhrasesService
    {
        public static readonly TagType[] PhraseTags = new[]
        {
            TagType.PhraseCharacter,
            TagType.PhraseSubject,
            TagType.PhraseType
        };

        readonly AppDbContext _dbContext;
        readonly IMapper _mapper;

        public PhrasesService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        async Task DeleteRemovedPhrase(string text)
        {
            var phrase = await _dbContext.Phrases
                .IgnoreQueryFilters()
                .Include(_ => _.Tags)
                .FirstOrDefaultAsync(_ => _.IsDeleted && _.Text == text);
            if (phrase == null) return;
            _dbContext.RemoveRange(phrase.Tags);
            _dbContext.Remove(phrase);
        }

        async Task SaveAndCheckUnique(CreatePhraseDto phrase)
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && sql.Number == 2601)
            {
                var text = phrase.Text.Length > 10 ? phrase.Text.Substring(0, 10) + "..." : phrase.Text;
                throw new AppException($"The phrase with the text '{text}' is already exists");
            }
        }

        public async Task<PhraseDto> Create(CreatePhraseDto phrase)
        {
            await DeleteRemovedPhrase(phrase.Text);
            var model = _mapper.Map<Phrase>(phrase);
            _dbContext.Add(model);
            await SaveAndCheckUnique(phrase);
            return _mapper.Map<PhraseDto>(model);
        }

        public async Task<PhraseDto> Modify(UpdatePhraseDto phrase)
        {
            await DeleteRemovedPhrase(phrase.Text);
            var model = _mapper.Map<Phrase>(phrase);
            var source = await _dbContext.Phrases
                .Include(_ => _.Tags)
                .FirstAsync(_ => _.Id == phrase.Id);

            _dbContext.ModifyCollection(source.Tags, model.Tags);

            if (source.Text == model.Text)
                source.UpdatedAt = DateTime.UtcNow;
            else
                source.Text = model.Text;

            await SaveAndCheckUnique(phrase);
            return _mapper.Map<PhraseDto>(source);
        }

        public Task Remove(Guid phraseId)
        {
            var phrase = _dbContext.Phrases.Find(phraseId);
            phrase.IsDeleted = true;
            return _dbContext.SaveChangesAsync();
        }

        public async Task<SimplePage<PhraseDto>> Search(PhrasesRequestCriteria criteria)
        {
            IQueryable<Phrase> query = _dbContext.Phrases.Include(_ => _.Tags);

            if (!string.IsNullOrWhiteSpace(criteria.Q))
                query = query.Where(_ => _.Text.Contains(criteria.Q));

            if (criteria.TagIds?.Any() == true)
            {
                Dictionary<TagType, Guid[]> GetTypeDict(IEnumerable<Tag> tags) => tags
                    .GroupBy(_ => _.Type)
                    .ToDictionary(g => g.Key, g => g.Select(_ => _.Id).ToArray());

                var phrasesTagsIdDict = await _dbContext.Tags
                    .Where(_ => PhraseTags.Contains(_.Type))
                    .ToDictionaryAsync(_ => _.Id, _ => _);
                var tagsTypeDict = GetTypeDict(criteria.TagIds.Select(_ => phrasesTagsIdDict[_]));
                IEnumerable<Guid[]> idsSets = tagsTypeDict.Values;
                var missedTypes = PhraseTags.Except(tagsTypeDict.Keys).ToHashSet();
                if (missedTypes.Count != 0)
                {
                    var missed = GetTypeDict(phrasesTagsIdDict.Values.Where(_ => missedTypes.Contains(_.Type)));
                    idsSets = idsSets.Concat(missed.Values);
                }

                var tagIds = idsSets.SelectMany(_ => _);

                query = query.Where(p => p.Tags.Select(_ => _.TagId).All(_ => tagIds.Contains(_)));
            }

            query = query.OrderByDescending(_ => _.UpdatedAt);

            if (criteria.FromItem != null)
            {
                var fromPhrase = await _dbContext.FindAsync<Phrase>(criteria.FromItem);
                query = query.Where(_ => _.UpdatedAt < fromPhrase.UpdatedAt);
            }

            var page = await query.GetPage(criteria);

            return _mapper.Map<SimplePage<PhraseDto>>(page);
        }

        public async Task<PhraseTagsDto> GetTags()
        {
            var tags = (await _dbContext.Tags
                .Where(_ => PhraseTags.Contains(_.Type))
                .ToArrayAsync())
                .GroupBy(_ => _.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .OrderBy(_ => _.Position)
                        .Select(_ => _mapper.Map<PhraseTagDto>(_))
                        .ToArray());

            return new PhraseTagsDto
            {
                Type = tags[TagType.PhraseType],
                Subject = tags[TagType.PhraseSubject],
                Character = tags[TagType.PhraseCharacter]
            };
        }
    }
}
