﻿using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using System;
using System.Threading.Tasks;

namespace CreoHp.Contracts
{
    public interface IPhrasesService
    {
        Task<SimplePage<PhraseDto>> Search(PhrasesRequestCriteria criteria);
        Task<PhraseDto> Create(CreatePhraseDto phrase);
        Task<PhraseDto> Modify(PhraseDto phrase);
        Task Remove(Guid phraseId);
        Task<PhraseTagsDto> GetTags();
    }
}
