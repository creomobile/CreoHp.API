using System;
using System.Linq;
using AutoMapper;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Phrases;
using CreoHp.Dto.Tags;
using CreoHp.Dto.Users;
using CreoHp.Models.Phrases;
using CreoHp.Models.Tags;
using CreoHp.Models.Users;
using Microsoft.Extensions.DependencyInjection;

namespace CreoHp.Services
{
    public static class AutoMapperConfig
    {
        public static void ConfigureMappings(IMapperConfigurationExpression config,
            IServiceProvider serviceProvider, Func<IMapper> mapper)
        {
            var rolesHelper = serviceProvider.GetRequiredService<IRolesHelper>();

            ConfigurePagination(config);
            ConfigureUsers(config, rolesHelper);
            ConfigureTags(config);
            ConfigurePhrases(config);
        }

        static void ConfigurePagination(IProfileExpression config)
        {
            config.CreateMap(typeof(SimplePage<>), typeof(SimplePage<>));
        }

        static void ConfigureUsers(IProfileExpression config, IRolesHelper rolesHelper)
        {
            config.CreateMap<SignUpDto, AppIdentityUser>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName.Trim()))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName.Trim()))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email.Trim()))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Trim()))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender))
                .ForAllOtherMembers(opt => opt.Ignore());

            config.CreateMap<AppIdentityUser, UserDto>();

            config.CreateMap<AppIdentityUser, UserWithRolesDto>()
                .IncludeBase<AppIdentityUser, UserDto>()
                .ForMember(d => d.Roles,
                    opt => opt.MapFrom(s => s.Roles.Select(_ => rolesHelper.GetRoleById(_.RoleId)).ToArray()));

            config.CreateMap<AppIdentityUser, SignedInDto>()
                .IncludeBase<AppIdentityUser, UserWithRolesDto>()
                .ForMember(d => d.Token, opt => opt.Ignore());
        }

        static void ConfigureTags(IProfileExpression config)
        {
            config.CreateMap<Tag, TagDto>();
        }

        static void ConfigurePhrases(IProfileExpression config)
        {
            config.CreateMap<Phrase, PhraseDto>()
                .ForMember(d => d.TagIds, opt => opt.MapFrom(s => s.Tags.Select(_ => _.TagId).ToArray()));
            config.CreateMap<CreatePhraseDto, Phrase>()
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.Text))
                .AfterMap((s, d) => d.Tags = s.TagIds.Select(_ => new PhraseTag { PhraseId = d.Id, TagId = _ }).ToArray())
                .ForAllOtherMembers(opt => opt.Ignore());
            config.CreateMap<PhraseDto, Phrase>()
                .IncludeBase<CreatePhraseDto, Phrase>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
        }
    }
}