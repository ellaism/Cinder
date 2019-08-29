using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Hosting;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Cinder.UI.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            services.AddSingleton<ICacheClient, InMemoryCacheClient>();
            services.AddHostedService<StatsBackgroundService>();
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddSingleton<IAddressService, AddressService>();
            services.AddSingleton<IBlockService, BlockService>();
            services.AddSingleton<ITransactionService, TransactionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseForwardedHeaders();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub<App>("app");
                endpoints.MapFallbackToPage("/_Host");
            });
            app.Use((ctx, next) =>
            {
                ctx.Request.Scheme = "https";

                return next();
            });
        }
    }
}
