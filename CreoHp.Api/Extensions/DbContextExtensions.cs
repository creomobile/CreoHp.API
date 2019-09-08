using CreoHp.Api.Services;
using CreoHp.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreoHp.Api.Extensions
{
    static class DbContextExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContextPool<AppDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(p => p.Default(WarningBehavior.Throw)
                    .Ignore(
                        CoreEventId.IncludeIgnoredWarning,
                        CoreEventId.RowLimitingOperationWithoutOrderByWarning,
                        CoreEventId.FirstWithoutOrderByAndFilterWarning)));

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                scope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();

            return app;
        }

        public static IApplicationBuilder InitAppData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                DbInitializer.Instance.Initialize(scope.ServiceProvider).Wait();

            return app;
        }
    }
}