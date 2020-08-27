using CreoHp.Api.Extensions;
using CreoHp.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CreoHp.Api
{
    public sealed class Startup
    {
        readonly ILogger<AppExceptionFilterAttribute> _appExceptionLogger;

        public Startup(IConfiguration configuration, ILogger<AppExceptionFilterAttribute> appExceptionLogger)
        {
            Configuration = configuration;
            _appExceptionLogger = appExceptionLogger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper()
                .AddDbContext(Configuration)
                .AddAuthenticationServices(Configuration)
                .AddDependencies(Configuration)
                .AddAppCors()
                .AddSwaggerServices()
                .AddControllers(p => p.Filters.Insert(0, new AppExceptionFilterAttribute(_appExceptionLogger)))
                .AddNewtonsoftJson(options =>
                {
                    var serializerSettings = options.SerializerSettings;
                    var converters = serializerSettings.Converters;

                    converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                    converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });

                    serializerSettings.NullValueHandling = NullValueHandling.Include;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAppCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage().UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseRouting()
                .UseStaticFiles()
                .UseSwagger()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(p => p.MapControllers())
                .ApplyMigrations()
                .InitAppData();
        }
    }
}