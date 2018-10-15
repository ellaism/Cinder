using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using EllaX.Core.Models;

namespace EllaX.Api.Infrastructure
{
    public class InMemoryStatistics
    {
        private readonly ConcurrentDictionary<string, Peer> _peers = new ConcurrentDictionary<string, Peer>();

        public IReadOnlyList<Peer> GetPeers()
        {
            return _peers.Values.ToImmutableList();
        }

        public void AddPeer(Peer peer)
        {
            if (peer.RemoteAddress.Contains("Handshake", StringComparison.InvariantCulture))
            {
                return;
            }

            _peers[peer.Id] = peer;
        }
    }
}
