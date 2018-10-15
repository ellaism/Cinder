using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Handlers
{
    public class NetworkHealthHandler : INotificationHandler<PeerNotification>
    {
        private readonly ILogger<NetworkHealthHandler> _logger;
        private readonly InMemoryStatistics _statistics;

        public NetworkHealthHandler(ILogger<NetworkHealthHandler> logger, InMemoryStatistics statistics)
        {
            _logger = logger;
            _statistics = statistics;
        }

        public Task Handle(PeerNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{@Peer}", notification.Peer);
            if (string.IsNullOrEmpty(notification.Peer.Id))
            {
                return Task.CompletedTask;
            }

            _statistics.AddPeer(notification.Peer);

            return Task.CompletedTask;
        }
    }
}
