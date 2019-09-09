using CreoHp.Common;
using CreoHp.Dto.Tags;
using System.Threading.Tasks;

namespace CreoHp.Contracts
{
    public interface ITagsService
    {
        Task<TagDto[]> GetTagsByTypes(params TagType[] types);
    }
}
