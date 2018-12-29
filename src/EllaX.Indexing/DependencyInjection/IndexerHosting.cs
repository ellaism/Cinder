using EllaX.Indexing.HostedServices;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class IndexerHosting
    {
        public static void AddIndexerHosting(this IServiceCollection services)
        {
            services.AddHostedService<IndexerHostedService>();
        }
    }
}
