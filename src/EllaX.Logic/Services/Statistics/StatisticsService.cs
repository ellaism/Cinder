using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core;
using EllaX.Core.Entities;
using EllaX.Core.Extensions;
using EllaX.Data;
using EllaX.Logic.Services.Statistics.Results;

namespace EllaX.Logic.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMapper _mapper;
        private readonly Repository _repository;

        public StatisticsService(IMapper mapper, Repository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task CreateRecentPeerSnapshotAsync(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default)
        {
            int count = _repository.Query<Peer>()
                .Where(peer => peer.LastSeenDate >= DateTime.UtcNow.AddMinutes(-Math.Abs(ageMinutes))).Count();
            _repository.Insert(Statistic.Create(StatisticType.PeerCountSnapshot.ToString(), count.ToString()));

            return Task.CompletedTask;
        }

        public Task<TDto> GetNetworkHealthAsync<TDto>(bool uniquesOnly = true,
            int ageMinutes = Consts.DefaultAgeMinutes, CancellationToken cancellationToken = default)
        {
            NetworkHealthResult response = GetNetworkHealth(uniquesOnly, ageMinutes);

            return Task.FromResult(_mapper.Map<TDto>(response));
        }

        private NetworkHealthResult GetNetworkHealth(bool uniquesOnly, int ageMinutes)
        {
            NetworkHealthResult response = new NetworkHealthResult();
            Peer[] peers = _repository.Query<Peer>()
                .Where(peer => peer.LastSeenDate >= DateTime.UtcNow.AddMinutes(-Math.Abs(ageMinutes))).ToArray()
                .OrderByDescending(peer => peer.LastSeenDate).ToArray();

            // get peer count before potentially removing records
            response.Count = peers.Length;

            if (!uniquesOnly)
            {
                response.Peers = peers;

                return response;
            }

            Dictionary<string, Peer> uniques = new Dictionary<string, Peer>();
            foreach (Peer peer in peers)
            {
                string key = $"{peer.Latitude}{peer.Longitude}".Md5();

                if (uniques.ContainsKey(key))
                {
                    continue;
                }

                uniques[key] = peer;

                response.Peers = uniques.Select(x => x.Value).ToArray();
            }

            return response;
        }
    }
}
