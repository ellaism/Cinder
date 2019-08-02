using System.Threading;
using System.Threading.Tasks;
using Cinder.Messages.Events;
using Microsoft.Extensions.Hosting;
using Rebus.Bus;

namespace Cinder.Api.Infrastructure.Hosting
{
    public class MessageService : BackgroundService
    {
        private readonly IBus _bus;

        public MessageService(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Subscribe<BlockEvent>();
        }
    }
}
