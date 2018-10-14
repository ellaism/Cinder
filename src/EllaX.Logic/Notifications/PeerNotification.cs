using EllaX.Core.Models;
using MediatR;

namespace EllaX.Logic.Notifications
{
    public class PeerNotification : INotification
    {
        public Peer Peer { get; set; }
    }
}
