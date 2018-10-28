using System.Collections.Generic;
using EllaX.Core.Entities;
using MediatR;

namespace EllaX.Logic.Notifications
{
    public class PeerNotification : INotification
    {
        public IEnumerable<Peer> Peers { get; set; }
    }
}
