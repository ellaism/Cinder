using Autofac;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Network;
using Flurl.Http.Configuration;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class Clients
    {
        public static void RegisterClients(this ContainerBuilder builder)
        {
            builder.RegisterType<PerBaseUrlFlurlClientFactory>().As<IFlurlClientFactory>().SingleInstance();
            builder.RegisterType<BlockchainClient>().As<IBlockchainClient>().InstancePerLifetimeScope();
            builder.RegisterType<NetworkClient>().As<INetworkClient>().InstancePerLifetimeScope();
        }
    }
}
