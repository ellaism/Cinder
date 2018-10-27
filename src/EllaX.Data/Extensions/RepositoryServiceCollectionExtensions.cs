using System;
using EllaX.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlockchainServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<RepositoryOptions>(options => options.ConnectionString = connectionString);

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton(typeof(Repository)));

            return services;
        }
    }
}
