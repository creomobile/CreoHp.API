using AutoMapper;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Models.Phrases;
using CreoHp.Models.Tags;
using CreoHp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreoHp.Services
{
    public sealed class PhrasesService : IPhrasesService
    {
        readonly AppDbContext _dbContext;
        readonly IMapper _mapper;

        public PhrasesService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public async Task<PhraseDto> CreatePhrase(CreatePhraseDto phrase)
        {
            var model = _mapper.Map<Phrase>(phrase);
            _dbContext.Add(model);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PhraseDto>(model);
        }

        public async Task<PhraseDto> ModifyPhrase(PhraseDto phrase)
        {
            var model = _mapper.Map<Phrase>(phrase);
            var source = await _dbContext.Phrases
                .Include(_ => _.Tags)
                .FirstAsync(_ => _.Id == phrase.Id);

            _dbContext.ModifyCollection(source.Tags, model.Tags);

            if (model.Text != source.Text)
                _dbContext.Update(model);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PhraseDto>(model);
        }

        public async Task<SimplePage<PhraseDto>> Search(PhrasesRequestCriteria criteria)
        {
            IQueryable<Phrase> query = _dbContext.Phrases;

            if (!string.IsNullOrWhiteSpace(criteria.Q))
                query = query.Where(_ => _.Text.Contains(criteria.Q));

            if (criteria.TagIds?.Any() == true)
            {
                Dictionary<TagType, Guid[]> GetTypeDict(IEnumerable<Tag> tags) => tags
                    .GroupBy(_ => _.Type)
                    .ToDictionary(g => g.Key, g => g.Select(_ => _.Id).ToArray());

                var phrasesTagsIdDict = await _dbContext.Tags
                    .Where(_ => TagsService.PhraseTags.Contains(_.Type))
                    .ToDictionaryAsync(_ => _.Id, _ => _);
                var tagsTypeDict = GetTypeDict(criteria.TagIds.Select(_ => phrasesTagsIdDict[_]));
                IEnumerable<Guid[]> idsSets = tagsTypeDict.Values;
                var missedTypes = TagsService.PhraseTags.Except(tagsTypeDict.Keys).ToHashSet();
                if (missedTypes.Count != 0)
                {
                    var missed = GetTypeDict(phrasesTagsIdDict.Values.Where(_ => missedTypes.Contains(_.Type)));
                    idsSets = idsSets.Concat(missed.Values);
                }

                var tagIds = idsSets.SelectMany(_ => _);

                query = query.Where(p => p.Tags.Select(_ => _.TagId).All(_ => tagIds.Contains(_)));
            }

            var page = await query.GetSimplePage(criteria);
            return _mapper.Map<SimplePage<PhraseDto>>(page);
        }
    }
}
