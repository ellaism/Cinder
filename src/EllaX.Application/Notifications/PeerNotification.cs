using System.Collections.Generic;
using EllaX.Core.Entities;
using MediatR;

namespace EllaX.Application.Notifications
{
    public class PeerNotification : INotification
    {
        public IEnumerable<Peer> Peers { get; set; }
    }
}
