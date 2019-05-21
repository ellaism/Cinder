using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.Clients.DependencyInjection
{
    public static class ClientConfiguration
    {
        public static void AddClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlockchainClientOptions>(configuration.GetSection("Indexing"));
        }
    }
}
