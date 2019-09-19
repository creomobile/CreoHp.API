using AutoMapper;
using CreoHp.Contracts;
using CreoHp.Repository;
using System;

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
    }
}
