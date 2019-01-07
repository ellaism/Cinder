using Autofac;
using EllaX.Clients;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class Clients
    {
        public static void RegisterClients(this ContainerBuilder builder)
        {
            builder.RegisterType<BlockchainClient>().As<IBlockchainClient>().InstancePerLifetimeScope();
        }
    }
}
