using CreoHp.Api.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Parse("192.168.1.10"), 5000);
                    options.Listen(IPAddress.Parse("192.168.1.10"), 5001, listenOptions =>
                    {
                        listenOptions.UseHttps(storeName: StoreName.My, "*.creomobile.com");
                    });
                })
                .UseConfiguration(ConfigurationExtensions.Create())
                .UseStartup<Startup>();
    }
}