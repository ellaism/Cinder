using System;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Entities;
using EllaX.Data;
using EllaX.Logic.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic
{
    public class PeerService : Service, IPeerService
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<PeerService> _logger;
        private readonly IMapper _mapper;
        private readonly Repository _repository;

        public PeerService(IMediator eventBus, ILogger<PeerService> logger, IMapper mapper,
            ILocationService locationService, Repository repository) : base(eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _locationService = locationService;
            _repository = repository;
        }

        public async Task ProcessPeerAsync(Peer peer)
        {
            if (peer.RemoteAddress.Contains("Handshake"))
            {
                return;
            }

            Uri uri = new Uri("http://" + peer.RemoteAddress);
            string peerId = peer.Id;
            _logger.LogDebug("Processing peer {Id} at address {Address}", peerId, peer.RemoteAddress);

            City city = await _locationService.GetCityByIpAsync(uri.Host);
            Peer updated = _mapper.Map(city, peer);

            Peer original = _repository.FirstOrDefault<Peer>(x => x.Id == peerId);
            if (original != null)
            {
                updated = _mapper.Map(updated, original);
            }

            _repository.Upsert(updated);
        }
    }
}
