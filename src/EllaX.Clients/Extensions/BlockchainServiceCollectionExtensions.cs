using System;
using EllaX.Clients;
using EllaX.Clients.Blockchain;
using Microsoft.Extensions.Configuration;
using Polly;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlockchainServiceCollectionExtensions
    {
        public static IServiceCollection AddBlockchainClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<BlockchainClientOptions>(configuration.GetSection("EllaX:Blockchain"));

            services.AddOptions();
            services.AddHttpClient<IBlockchainClient, BlockchainClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            return services;
        }
    }
}
