using System;
using Autofac;
using AutoMapper;
using EllaX.Api.Infrastructure;
using EllaX.Api.Infrastructure.Hosting;
using EllaX.Logic;
using EllaX.Logic.Clients;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace EllaX.Api
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
            services.AddHttpClient<IBlockchainClient, BlockchainClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
            services.AddMediatR();
            services.AddAutoMapper(cfg => cfg.AddProfiles(typeof(Service).Assembly));

            // hosted services
            services.AddHostedService<NetworkHealthHostedService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryStatistics>().SingleInstance();
            builder.RegisterType<BlockchainService>().As<IBlockchainService>().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
