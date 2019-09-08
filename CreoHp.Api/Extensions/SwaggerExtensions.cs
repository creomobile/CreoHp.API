using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CreoHp.Api.Extensions
{
    static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
            => services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "API", Version = "v1"}); });

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
            => SwaggerBuilderExtensions.UseSwagger(app)
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
    }
}