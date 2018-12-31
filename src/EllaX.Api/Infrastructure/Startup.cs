using Autofac;
using EllaX.Builder;
using EllaX.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EllaX.Api.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ellax
            services.AddDatabase(Configuration);
            services.AddMapper();
            services.AddMediation();

            // mvc
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddAuthorizationPolicies();
            services.AddApiDocumentation();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterApplication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseErrorHandling();
            app.UseCors(options => options.AllowAnyOrigin());
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseApiDocumentation(provider);
        }
    }
}
