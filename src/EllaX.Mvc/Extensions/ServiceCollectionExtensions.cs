using System;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Network;
using EllaX.Data;
using EllaX.Logic.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace EllaX.Mvc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEllaX(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<RepositoryOptions>(options =>
                options.ConnectionString = configuration.GetConnectionString("RepositoryConnection"));
            services.Configure<LocationOptions>(options =>
                options.ConnectionString = configuration.GetConnectionString("GeoIpConnection"));
            services.Configure<BlockchainClientOptions>(configuration.GetSection("EllaX:Blockchain"));
            services.Configure<NetworkClientOptions>(configuration.GetSection("EllaX:Network"));

            services.AddOptions();
            services.AddHttpClient<IBlockchainClient, BlockchainClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
            services.AddHttpClient<INetworkClient, NetworkClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            return services;
        }
    }
}
