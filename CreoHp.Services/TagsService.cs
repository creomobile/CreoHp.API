using AutoMapper;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Tags;
using CreoHp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CreoHp.Services
{
    public sealed class TagsService : ITagsService
    {
        readonly AppDbContext _dbContext;
        readonly IMapper _mapper;

        public TagsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public Task<TagDto[]> GetTagsByTypes(params TagType[] types) => _dbContext.Tags
            .Where(_ => types.Contains(_.Type))
            .ToAsyncEnumerable()
            .Select(_ => _mapper.Map<TagDto>(_))
            .ToArray();
    }
}
