using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Notifications;
using EllaX.Logic.Services;
using MediatR;

namespace EllaX.Logic.Handlers
{
    public class PeerNotificationHandler : INotificationHandler<PeerNotification>
    {
        private readonly IPeerService _peerService;

        public PeerNotificationHandler(IPeerService peerService)
        {
            _peerService = peerService;
        }

        public Task Handle(PeerNotification notification, CancellationToken cancellationToken)
        {
            return _peerService.ProcessPeersAsync(notification.Peers, cancellationToken);
        }
    }
}
