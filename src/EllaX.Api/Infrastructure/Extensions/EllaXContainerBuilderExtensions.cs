using Autofac;
using EllaX.Data;
using EllaX.Indexing;
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
            builder.RegisterType<Repository>().SingleInstance();

            // services
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            builder.RegisterType<BlockchainService>().As<IBlockchainService>().InstancePerLifetimeScope();
            builder.RegisterType<PeerService>().As<IPeerService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerLifetimeScope();

            // indexer
            builder.RegisterType<Indexer>().As<IIndexer>().InstancePerLifetimeScope();
        }
    }
}
