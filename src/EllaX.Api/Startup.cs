using System;
using System.IO;
using System.Reflection;
using Autofac;
using AutoMapper;
using EllaX.Api.Infrastructure.Hosting;
using EllaX.Data;
using EllaX.Data.Options;
using EllaX.Logic.Clients;
using EllaX.Logic.Clients.Options;
using EllaX.Logic.Options;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Location;
using EllaX.Logic.Services.Statistics;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Polly;
using Swashbuckle.AspNetCore.Swagger;

namespace EllaX.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static string XmlCommentsFilePath
        {
            get
            {
                string basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";

                return Path.Combine(basePath, fileName);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configuration
            services.Configure<RepositoryOptions>(options =>
                options.ConnectionString = Configuration.GetConnectionString("RepositoryConnection"));
            services.Configure<LocationOptions>(options =>
                options.ConnectionString = Configuration.GetConnectionString("GeoIpConnection"));
            services.Configure<BlockchainClientOptions>(Configuration.GetSection("Blockchain"));
            services.Configure<NetworkClientOptions>(Configuration.GetSection("Network"));

            // http clients
            services.AddHttpClient<IBlockchainClient, BlockchainClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
            services.AddHttpClient<INetworkClient, NetworkClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            // messaging and mapping
            services.AddMediatR();
            services.AddAutoMapper(cfg => cfg.AddProfiles(typeof(Service)));

            // hosted services
            services.AddHostedService<NetworkHealthHostedService>();

            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore().AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            // mvc
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddSwaggerGen(options =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                IApiVersionDescriptionProvider provider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                options.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Repository>().SingleInstance();
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();

            builder.RegisterType<BlockchainService>().As<IBlockchainService>().InstancePerLifetimeScope();
            builder.RegisterType<PeerService>().As<IPeerService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerLifetimeScope();
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

            app.UseCors(options => options.AllowAnyOrigin());
            //app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            Info info = new Info
            {
                Title = $"EllaX API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "An explorer and indexer for the Ellaism blockchain.",
                Contact = new Contact
                {
                    Name = "Nodestry", Email = "hi@nodestry.com", Url = "https://github.com/Nodestry/EllaX"
                },
                License = new License {Name = "MIT", Url = "https://opensource.org/licenses/MIT"}
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
