using System;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic
{
    public class PeerService : Service, IPeerService
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<BlockchainService> _logger;
        private readonly IMapper _mapper;
        private readonly IStatisticsService _statisticsService;

        public PeerService(IMediator eventBus, ILogger<BlockchainService> logger, IMapper mapper,
            IStatisticsService statisticsService, ILocationService locationService) : base(eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _statisticsService = statisticsService;
            _locationService = locationService;
        }

        public Task ProcessPeerAsync(Peer peer)
        {
            if (string.IsNullOrEmpty(peer.Id))
            {
                return Task.CompletedTask;
            }

            Uri uri = new Uri("http://" + peer.RemoteAddress);
            City city = _locationService.GetCityByIp(uri.Host);
            peer = _mapper.Map(city, peer);

            _logger.LogDebug("{@Peer}", peer);
            _statisticsService.AddPeer(peer);

            return Task.CompletedTask;
        }
    }
}
