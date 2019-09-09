using CreoHp.Api.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CreoHp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://localhost:5000", "https://localhost:5001", "http://192.168.1.10:5000", "https://192.168.1.10:5001")
                .UseConfiguration(ConfigurationExtensions.Create())
                .UseStartup<Startup>();
    }
}