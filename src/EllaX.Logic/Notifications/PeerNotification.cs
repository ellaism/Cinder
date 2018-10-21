using EllaX.Core.Entities;
using MediatR;

namespace EllaX.Logic.Notifications
{
    public class PeerNotification : INotification
    {
        public Peer Peer { get; set; }
    }
}
