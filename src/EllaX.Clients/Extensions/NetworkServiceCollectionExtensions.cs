using System;
using EllaX.Clients;
using EllaX.Clients.Network;
using Microsoft.Extensions.Configuration;
using Polly;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class NetworkServiceCollectionExtensions
    {
        public static IServiceCollection AddNetworkClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<NetworkClientOptions>(configuration.GetSection("EllaX:Network"));

            services.AddOptions();
            services.AddHttpClient<INetworkClient, NetworkClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            return services;
        }
    }
}
