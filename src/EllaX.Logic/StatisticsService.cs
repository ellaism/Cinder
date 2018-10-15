using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;

namespace EllaX.Logic
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMapper _mapper;
        private readonly ConcurrentDictionary<string, Peer> _peers = new ConcurrentDictionary<string, Peer>();

        public StatisticsService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<IReadOnlyList<Health>> GetHealthAsync()
        {
            IReadOnlyList<Health> health = _mapper.Map<IReadOnlyList<Health>>(_peers.Values.ToImmutableList());

            return Task.FromResult(health);
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
