using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

namespace CreoHp.Api.Extensions
{
    static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CreoHp API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                        new ApiKeyScheme
                        {
                            In = "header",
                            Description = "Please enter into field the word 'Bearer' following by space and JWT",
                            Name = "Authorization",
                            Type = "apiKey"
                        });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    ["Bearer"] = Enumerable.Empty<string>()
                });
            });

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
            => SwaggerBuilderExtensions.UseSwagger(app)
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
    }
}