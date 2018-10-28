using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Entities;
using EllaX.Data;
using EllaX.Logic.Services.Location;
using EllaX.Logic.Services.Location.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Services
{
    public class PeerService : Service, IPeerService
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<PeerService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public PeerService(IMediator eventBus, ILogger<PeerService> logger, IMapper mapper,
            ILocationService locationService, IRepository repository) : base(eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _locationService = locationService;
            _repository = repository;
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

            _logger.LogInformation("Found {Count} peers", validPeers.Count);

            try
            {
                await _repository.SaveBatchAsync(validPeers, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PeerService -> ProcessPeersAsync");
            }
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
    }
}
