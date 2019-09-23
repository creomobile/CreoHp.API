using CreoHp.Api.Attributes;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CreoHp.Api.Controllers
{
    [ApiController, AuthorizeHp(UserRole.Admin, UserRole.Editor), Route("api/phrases")]
    public class PhrasesController : ControllerBase
    {
        readonly IPhrasesService _phrasesService;

        public PhrasesController(IPhrasesService phrasesService)
        {
            _phrasesService = phrasesService ?? throw new ArgumentException(nameof(phrasesService));
        }

        [HttpGet]
        public Task<SimplePage<PhraseDto>> Search([FromQuery] PhrasesRequestCriteria criteria) => _phrasesService.Search(criteria);

        [HttpPost]
        public Task<PhraseDto> CreatePhrase(CreatePhraseDto phrase) => _phrasesService.Create(phrase);

        [HttpPut]
        public Task<PhraseDto> ModifyPhrase(UpdatePhraseDto phrase) => _phrasesService.Modify(phrase);

        [HttpDelete]
        public Task Remove([FromQuery] Guid phraseId) => _phrasesService.Remove(phraseId);

        [HttpGet("tags")]
        public Task<PhraseTagsDto> GetPhrasesTags() => _phrasesService.GetTags();

    }
}
