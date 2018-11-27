using Autofac;
using EllaX.Data;
using EllaX.Logic.Services;

namespace EllaX.Mvc.Extensions
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
