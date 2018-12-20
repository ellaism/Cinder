using EllaX.Indexer.Infrastructure.Indexing;

// ReSharper disable once CheckNamespace
namespace EllaX.Indexer.Infrastructure.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterEllaXIndexers(this ContainerBuilder builder)
        {
            // indexers
            builder.RegisterType<BlockchainIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<IndexerManager>().As<IIndexerManager>().InstancePerLifetimeScope();
        }
    }
}
