using Cinder.Api.Infrastructure;
using Cinder.Api.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Messaging
    {
        public static void AddMessaging(this IServiceCollection services)
        {
            services.AutoRegisterHandlersFromAssemblyOf<BlockEventHandler>();
            services.AddRebus((configurer, provider) =>
            {
                IOptions<Settings> options = provider.GetService<IOptions<Settings>>();
                configurer.Logging(l => l.Serilog())
                    .Transport(transport => transport.UseRabbitMq(options.Value.Queue.ConnectionString, options.Value.Queue.Name))
                    .Routing(routing => routing.TypeBased());

                return configurer;
            });
        }
    }
}
