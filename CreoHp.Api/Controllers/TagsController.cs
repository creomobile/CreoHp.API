using CreoHp.Api.Attributes;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Tags;
using CreoHp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CreoHp.Api.Controllers
{
    [ApiController, AuthorizeHp(UserRole.Admin, UserRole.Editor), Route("api/tags")]
    public class TagsController : ControllerBase
    {
        readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService ?? throw new ArgumentException(nameof(tagsService));
        }

        [HttpGet("phrases")]
        public Task<TagDto[]> GetPhrasesTags() => _tagsService.GetTagsByTypes(TagsService.PhraseTags);
    }
}
