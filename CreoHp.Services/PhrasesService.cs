using AutoMapper;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using CreoHp.Models.Phrases;
using CreoHp.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
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
                .Include(_ => _.Collection)
                .FirstOrDefaultAsync(_ => _.IsDeleted && _.Text == text);
            if (phrase == null) return;
            _dbContext.RemoveRange(phrase.Tags);
            if (phrase.Collection != null) _dbContext.Remove(phrase.Collection);
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

        public async Task Remove(Guid phraseId)
        {
            var phrase = await _dbContext.Phrases
                .Include(p => p.Collection)
                .FirstOrDefaultAsync(p => p.Id == phraseId);

            phrase.IsDeleted = true;
            if (phrase.Collection != null) phrase.Collection.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Page<PhraseDto>> Search(PhrasesRequestCriteria criteria)
        {
            IQueryable<Phrase> query = _dbContext.Phrases.Include(_ => _.Tags);

            if (!string.IsNullOrWhiteSpace(criteria.Q))
                query = query.Where(_ => _.Text.Contains(criteria.Q));

            if (criteria.TagIds?.Any() == true)
            {
                var idDict = await _dbContext.Tags
                    .Where(_ => PhraseTags.Contains(_.Type))
                    .ToDictionaryAsync(_ => _.Id, _ => _);
                var typeDict = criteria.TagIds
                    .GroupBy(_ => idDict[_].Type)
                    .ToDictionary(_ => _.Key, _ => _.ToArray());

                foreach (var ids in typeDict.Values)
                    query = query.Where(p => p.Tags.Any(t => ids.Contains(t.TagId)));
            }

            query = query.OrderByDescending(_ => _.UpdatedAt);

            if (criteria.FromItem != null)
            {
                var fromPhrase = await _dbContext.FindAsync<Phrase>(criteria.FromItem);
                query = query.Where(_ => _.UpdatedAt < fromPhrase.UpdatedAt);
            }

            var page = await query.GetPage(criteria);

            return _mapper.Map<Page<PhraseDto>>(page);
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
