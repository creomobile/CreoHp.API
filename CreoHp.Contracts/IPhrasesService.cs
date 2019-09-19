using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using System.Threading.Tasks;

namespace CreoHp.Contracts
{
    public interface IPhrasesService
    {
        Task<SimplePage<PhraseDto>> Search(PhrasesRequestCriteria criteria);
        Task<PhraseDto> CreatePhrase(CreatePhraseDto phrase);
        Task<PhraseDto> ModifyPhrase(PhraseDto phrase);
        Task<PhraseTagsDto> GetTags();
    }
}
