using System;
using CreoHp.Models.Users;
using CreoHp.Repository;
using CreoHp.Services.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CreoHp.Api.Extensions
{
    static class AuthExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var authConfig = configuration.GetSectionValue<AuthConfig>();

            services
                .AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                    options.User.AllowedUserNameCharacters += '+';
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(s =>
                {
                    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    s.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = authConfig.SecurityKey,
                        ValidIssuer = authConfig.Issuer,
                        ValidAudience = authConfig.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(30)
                    };
                });

            return services;
        }
    }
}