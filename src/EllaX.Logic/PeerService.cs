using System;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;
using EllaX.Data;
using MediatR;

namespace EllaX.Logic
{
    public class PeerService : Service, IPeerService
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;
        private readonly Repository _repository;

        public PeerService(IMediator eventBus, IMapper mapper, ILocationService locationService,
            Repository repository) : base(eventBus)
        {
            _mapper = mapper;
            _locationService = locationService;
            _repository = repository;
        }

        public Task ProcessPeerAsync(Peer peer)
        {
            if (peer.RemoteAddress.Contains("Handshake", StringComparison.InvariantCulture))
            {
                return Task.CompletedTask;
            }

            Uri uri = new Uri("http://" + peer.RemoteAddress);
            string peerId = peer.Id;
            City city = _locationService.GetCityByIp(uri.Host);
            Peer updated = _mapper.Map(city, peer);

            Peer original = _repository.FirstOrDefault<Peer>(x => x.Id == peerId);
            if (original != null)
            {
                updated = _mapper.Map(updated, original);
            }

            _repository.Upsert(updated);

            return Task.CompletedTask;
        }
    }
}
