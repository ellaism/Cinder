using Autofac;
using EllaX.Data;
using EllaX.Logic.Indexing;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Location;
using EllaX.Logic.Services.Statistics;

namespace EllaX.Api.Infrastructure.Extensions
{
    public static class EllaXContainerBuilderExtensions
    {
        public static void RegisterEllaXTypes(this ContainerBuilder builder)
        {
            // repository
            builder.RegisterType<Repository>().As<IRepository>().SingleInstance();

            // services
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            builder.RegisterType<BlockchainService>().As<IBlockchainService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerLifetimeScope();

            // indexers
            builder.RegisterType<BlockchainIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsIndexer>().As<IIndexer>().InstancePerLifetimeScope();
            builder.RegisterType<IndexerManager>().As<IIndexerManager>().InstancePerLifetimeScope();
        }
    }
}
