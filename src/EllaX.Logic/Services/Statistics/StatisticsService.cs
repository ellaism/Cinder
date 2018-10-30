using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core;
using EllaX.Core.Entities;
using EllaX.Core.Exceptions;
using EllaX.Core.Extensions;
using EllaX.Data;
using EllaX.Logic.Notifications;
using EllaX.Logic.Services.Location;
using EllaX.Logic.Services.Location.Results;
using EllaX.Logic.Services.Statistics.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<StatisticsService> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IRepository _repository;

        public StatisticsService(ILogger<StatisticsService> logger, IMapper mapper, IMediator mediator,
            IRepository repository, ILocationService locationService)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _repository = repository;
            _locationService = locationService;
        }

        public async Task CreateRecentPeerSnapshotAsync(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default)
        {
            int count = _repository.Provider.Query<Peer>()
                .Where(peer => peer.LastSeenDate >= DateTime.UtcNow.AddMinutes(-Math.Abs(ageMinutes))).Count();

            try
            {
                // save record to repository
                await _repository.SaveAsync(
                    Statistic.Create(StatisticType.RecentPeerSnapshot.ToString(), count.ToString()), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e,
                    $"{nameof(StatisticsService)} -> {nameof(CreateRecentPeerSnapshotAsync)} -> When saving recent peer snapshot to repository");

                throw new LoggedException(e);
            }
        }

        public async Task ProcessPeersAsync(IEnumerable<Peer> peers, CancellationToken cancellationToken = default)
        {
            IEnumerable<Peer> uniquePeers = peers.GroupBy(peer => peer.Id).Select(peer => peer.First());
            List<Peer> validPeers = new List<Peer>();

            foreach (Peer peer in uniquePeers)
            {
                Peer validated = await ProcessPeerAsync(peer, cancellationToken);

                if (validated == null)
                {
                    continue;
                }

                validPeers.Add(validated);
            }

            if (!validPeers.Any())
            {
                return;
            }

            _logger.LogInformation("Processed {Count} unique peers", validPeers.Count);

            try
            {
                // save changes to repository
                await _repository.SaveBatchAsync(validPeers, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e,
                    $"{nameof(StatisticsService)} -> {nameof(ProcessPeersAsync)} -> When saving peers to repository");

                throw new LoggedException(e);
            }

            try
            {
                // publish notification to mediator
                await _mediator.Publish(new PeerNotification {Peers = validPeers}, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e,
                    $"{nameof(StatisticsService)} -> {nameof(ProcessPeersAsync)} -> When publishing peer notification");

                throw new LoggedException(e);
            }
        }

        public Task<TDto> GetNetworkHealthAsync<TDto>(bool uniquesOnly = true,
            int ageMinutes = Consts.DefaultAgeMinutes, CancellationToken cancellationToken = default)
        {
            NetworkHealthResult response = GetNetworkHealth(uniquesOnly, ageMinutes);

            return Task.FromResult(_mapper.Map<TDto>(response));
        }

        private async Task<Peer> ProcessPeerAsync(Peer peer, CancellationToken cancellationToken = default)
        {
            if (peer.RemoteAddress.Contains("Handshake"))
            {
                return null;
            }

            Uri uri = new Uri("http://" + peer.RemoteAddress);
            string peerId = peer.Id;
            _logger.LogDebug("Processing peer {Id} at address {Address}", peerId, peer.RemoteAddress);

            CityResult result = await _locationService.GetCityByIpAsync(uri.Host, cancellationToken);
            Peer updated = _mapper.Map(result, peer);

            Peer original = _repository.Provider.FirstOrDefault<Peer>(x => x.Id == peerId);
            if (original != null)
            {
                updated = _mapper.Map(updated, original);
            }

            return updated;
        }

        private NetworkHealthResult GetNetworkHealth(bool uniquesOnly, int ageMinutes)
        {
            NetworkHealthResult response = new NetworkHealthResult();
            Peer[] peers = _repository.Provider.Query<Peer>()
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
