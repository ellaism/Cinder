using Cinder.Indexer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Message
    {
        public static void AddMessaging(this IServiceCollection services)
        {
            services.AddRebus((configurer, provider) =>
            {
                IOptions<Settings> options = provider.GetService<IOptions<Settings>>();
                configurer.Logging(l => l.Serilog())
                    .Transport(transport => transport.UseRabbitMqAsOneWayClient(options.Value.Queue.ConnectionString))
                    .Routing(routing => routing.TypeBased());

                return configurer;
            });
        }
    }
}
