using Autofac;
using EllaX.Data;
using EllaX.Logic.Services;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterEllaXTypes(this ContainerBuilder builder)
        {
            // repository
            builder.RegisterType<Repository>().As<IRepository>().SingleInstance();

            // services
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            builder.RegisterType<BlockchainService>().As<IBlockchainService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerLifetimeScope();
        }
    }
}
