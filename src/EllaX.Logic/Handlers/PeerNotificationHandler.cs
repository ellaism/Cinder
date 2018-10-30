using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Handlers
{
    public class PeerNotificationHandler : INotificationHandler<PeerNotification>
    {
        private readonly ILogger<PeerNotificationHandler> _logger;

        public PeerNotificationHandler(ILogger<PeerNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PeerNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Peer notification received, {Count} peers found", notification.Peers.Count());

            return Task.CompletedTask;
        }
    }
}
