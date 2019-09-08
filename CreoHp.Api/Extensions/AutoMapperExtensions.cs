using AutoMapper;
using CreoHp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CreoHp.Api.Extensions
{
    static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddSingleton(serviceProvider =>
            {
                IMapper mapper = null;

                // ReSharper disable once AccessToModifiedClosure
                IMapper GetMapper() => mapper;

                var mapperConfig = new MapperConfiguration(config =>
                    AutoMapperConfig.ConfigureMappings(config, serviceProvider, GetMapper));
                mapperConfig.AssertConfigurationIsValid();
                mapper = mapperConfig.CreateMapper();
                return mapper;
            });
        }
    }
}