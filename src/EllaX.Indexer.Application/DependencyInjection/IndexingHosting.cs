using EllaX.Indexer.Application.Hosting;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class IndexingHosting
    {
        public static void AddIndexingHosting(this IServiceCollection services)
        {
            services.AddHostedService<IndexingHostedService>();
        }
    }
}
