using Cinder.Indexer.Infrastructure;
using Cinder.Indexer.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.BlockchainProcessing.ProgressRepositories;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Database
    {
        public static void AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IIndexerRepositoryFactory>(sp =>
            {
                IOptions<Settings> options = sp.GetService<IOptions<Settings>>();

                return RepositoryFactory.Create(options.Value.Database);
            });
            services.AddSingleton<IAddressTransactionRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<AddressTransactionRepository>());
            services.AddSingleton<IBlockRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<BlockRepository>());
            services.AddSingleton<IContractRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<ContractRepository>());
            services.AddSingleton<ITransactionLogRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<TransactionLogRepository>());
            services.AddSingleton<ITransactionRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<TransactionRepository>());
            services.AddSingleton<IBlockProgressRepository>(sp =>
                sp.GetService<IIndexerRepositoryFactory>().CreateRepository<BlockProgressRepository>());
        }
    }
}
