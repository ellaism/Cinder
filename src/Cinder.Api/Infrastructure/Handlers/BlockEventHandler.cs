using System.Threading.Tasks;
using Cinder.Messages.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Cinder.Api.Infrastructure.Handlers
{
    public class BlockEventHandler : IHandleMessages<BlockEvent>
    {
        private readonly ILogger<BlockEventHandler> _logger;

        public BlockEventHandler(ILogger<BlockEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BlockEvent message)
        {
            _logger.LogDebug("Message received: {@Message}", message);

            return Task.CompletedTask;
        }
    }
}
