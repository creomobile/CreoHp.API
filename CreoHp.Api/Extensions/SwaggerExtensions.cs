using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CreoHp.Api.Extensions
{
    static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CreoHp API", Version = "v1" });
                var scheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                };
                c.AddSecurityDefinition("Bearer", scheme);

                var securityRequirement = new OpenApiSecurityRequirement { { scheme, new[] { "Bearer" } } };
                c.AddSecurityRequirement(securityRequirement);
            });

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
            => SwaggerBuilderExtensions.UseSwagger(app)
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
    }
}