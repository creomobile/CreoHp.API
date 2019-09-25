using CreoHp.Api.Extensions;
using CreoHp.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CreoHp.Api
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

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
                .AddMvc(options =>
                {
                    options.Filters.Insert(0, new AppExceptionFilterAttribute(
                        services.BuildServiceProvider().GetService<ILogger<AppExceptionFilterAttribute>>()));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
                    {
                        DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"
                    });
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                //.UseHttpsRedirection()
                .UseStaticFiles()
                .UseSwagger()
                .UseAuthentication()
                .UseMvc()
                .ApplyMigrations()
                .InitAppData();
        }
    }
}