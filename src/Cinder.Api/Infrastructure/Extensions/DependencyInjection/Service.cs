using Cinder.Api.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Service
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMinerLookupService, MinerLookupService>();
        }
    }
}
