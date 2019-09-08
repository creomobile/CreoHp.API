using System.IO;
using Microsoft.Extensions.Configuration;

namespace CreoHp.Api.Extensions
{
    static class ConfigurationExtensions
    {
        public static IConfigurationRoot Create() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        static string GetKey<TSection>()
        {
            const string suffix = "Config";
            var result = typeof(TSection).Name;
            result = char.ToLower(result[0]) + result.Substring(1);
            return result.EndsWith(suffix)
                ? result.Substring(0, result.Length - suffix.Length)
                : result;
        }

        public static IConfigurationSection GetSection<TSection>(this IConfiguration configuration)
            => configuration.GetSection(GetKey<TSection>());

        public static TSection GetSectionValue<TSection>(this IConfiguration configuration) where TSection : new()
        {
            var result = new TSection();
            configuration.GetSection<TSection>().Bind(result);
            return result;
        }
    }
}