using Cinder.Extensions.Builder;
using Cinder.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Cinder.Api.Infrastructure
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
            services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
            services.AddErrorHandling();
            services.AddOptions(Configuration);
            services.AddDatabase();
            services.AddHosting();
            //services.AddMessaging();
            services.AddMediator();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseErrorHandling();
            //app.UseMessaging();
            app.UseMvc();
        }
    }
}
