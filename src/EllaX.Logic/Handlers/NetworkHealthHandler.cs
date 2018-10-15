using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Notifications;
using MediatR;

namespace EllaX.Logic.Handlers
{
    public class NetworkHealthHandler : INotificationHandler<PeerNotification>
    {
        private readonly IPeerService _peerService;

        public NetworkHealthHandler(IPeerService peerService)
        {
            _peerService = peerService;
        }

        public Task Handle(PeerNotification notification, CancellationToken cancellationToken)
        {
            return _peerService.ProcessPeerAsync(notification.Peer);
        }
    }
}
