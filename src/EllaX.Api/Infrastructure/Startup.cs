using EllaX.Extensions.Builder;
using EllaX.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.DependencyInjection;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace EllaX.Api.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddErrorHandling();
            services.AddMapper();
            services.AddMediation();
            services.AddRavenDbDocStore();
            services.AddRavenDbAsyncSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddValidation();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfigurationProvider mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // validate mapper configuration
            mapper.AssertConfigurationIsValid();

            app.UseErrorHandling();
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
