using CreoHp.Api.Services;
using CreoHp.Contracts;
using CreoHp.Repository;
using CreoHp.Services;
using CreoHp.Services.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreoHp.Api.Extensions
{
    static class DependenciesExtensions
    {
        public static IServiceCollection AddDependencies(
            this IServiceCollection services, IConfiguration configuration) => services
            .ConfigureOptions(configuration)
            .AddSingleton<IRolesHelper>(DbInitializer.Instance)
            .AddSingleton<IDbInterceptor, DbInterceptor>()
            .AddScoped<IPrincipalService, PrincipalService>()
            .AddScoped<IUsersService, UsersService>();

        static IServiceCollection ConfigureOptions(this IServiceCollection services
            , IConfiguration configuration)
            => services.AddOptions()
                .Configure<AuthConfig>(configuration.GetSection<AuthConfig>());
    }
}