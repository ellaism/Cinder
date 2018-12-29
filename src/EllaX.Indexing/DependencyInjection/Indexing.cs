using Autofac;
using EllaX.Indexing;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class Indexing
    {
        public static void RegisterIndexing(this ContainerBuilder builder)
        {
            builder.RegisterType<BlockchainIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<IndexerManager>().As<IIndexerManager>().InstancePerLifetimeScope();
        }
    }
}
