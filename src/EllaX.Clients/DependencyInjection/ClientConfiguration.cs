﻿using EllaX.Clients.Blockchain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class ClientConfiguration
    {
        public static void AddClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlockchainClientOptions>(configuration.GetSection("Indexing"));
        }
    }
}
