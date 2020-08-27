using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CreoHp.Api.Extensions
{
    static class CorsExtensions
    {
        const string PolicyName = "AllowAll";

        public static IServiceCollection AddAppCors(this IServiceCollection services) =>
            services.AddCors(options => options.AddPolicy(PolicyName, p => p
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromDays(1))));
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder builder) =>
            builder.UseCors(PolicyName);
    }
}
