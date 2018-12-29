using EllaX.Api.Infrastructure.HostedServices;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class HostedServices
    {
        public static void AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<IndexerHostedService>();
        }
    }
}
