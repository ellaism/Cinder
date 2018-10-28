using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Responses.Parity.NetPeers;
using EllaX.Core.Entities;
using EllaX.Logic.Notifications;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Statistics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Threading.Tasks.Task;

namespace EllaX.Logic.Indexing
{
    public class StatisticsIndexer : IIndexer
    {
        private readonly ILogger<StatisticsIndexer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private DateTimeOffset _lastPeerCountSnapshot = DateTimeOffset.MinValue;

        public StatisticsIndexer(IServiceScopeFactory serviceScopeFactory, ILogger<StatisticsIndexer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Statistics indexer starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Statistics indexer polling");
                await DoWOrk(cancellationToken);
            }
        }

        private async Task DoWOrk(CancellationToken cancellationToken = default)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IMapper mapper = scope.ServiceProvider.GetService<IMapper>();
                IMediator mediator = scope.ServiceProvider.GetService<IMediator>();
                IBlockchainService blockchainService = scope.ServiceProvider.GetService<IBlockchainService>();
                IStatisticsService statisticsService = scope.ServiceProvider.GetService<IStatisticsService>();

                await CheckNetworkHealthAsync(mediator, mapper, blockchainService, cancellationToken);

                if (_lastPeerCountSnapshot == DateTimeOffset.MinValue ||
                    DateTimeOffset.UtcNow - _lastPeerCountSnapshot > TimeSpan.FromMinutes(60))
                {
                    await Snapshot(statisticsService, cancellationToken);
                }
            }

            await Delay(TimeSpan.FromMinutes(2), cancellationToken);
        }

        private async Task Snapshot(IStatisticsService statisticsService, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Taking snapshot of recent peers");

            await statisticsService.CreateRecentPeerSnapshotAsync(cancellationToken: cancellationToken);
            _lastPeerCountSnapshot = DateTimeOffset.UtcNow;
        }

        private async Task CheckNetworkHealthAsync(IMediator mediator, IMapper mapper,
            IBlockchainService blockchainService, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Retrieving latest network health snapshot");

            IReadOnlyCollection<PeerItem> peers = await blockchainService.GetPeersAsync(cancellationToken);
            if (!peers.Any())
            {
                return;
            }

            await mediator.Publish(new PeerNotification {Peers = mapper.Map<IEnumerable<Peer>>(peers)},
                cancellationToken);
        }
    }
}
