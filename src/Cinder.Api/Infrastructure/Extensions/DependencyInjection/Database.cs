using Cinder.Api.Infrastructure;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Database
    {
        public static void AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IRepositoryFactory>(sp =>
            {
                IOptions<Settings> options = sp.GetService<IOptions<Settings>>();

                return RepositoryFactory.Create(options.Value.Database);
            });
            services.AddSingleton<IBlockReadOnlyRepository>(sp =>
                sp.GetService<IRepositoryFactory>().CreateRepository<BlockReadOnlyRepository>());
            services.AddSingleton<ITransactionReadOnlyRepository>(sp =>
                sp.GetService<IRepositoryFactory>().CreateRepository<TransactionReadOnlyRepository>());
        }
    }
}
