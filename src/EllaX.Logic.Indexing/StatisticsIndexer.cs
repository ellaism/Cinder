using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Responses.Parity.NetPeers;
using EllaX.Core.Entities;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Indexing
{
    public class StatisticsIndexer : IIndexer
    {
        private readonly ILogger<StatisticsIndexer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private DateTimeOffset _lastPeerSnapshot = DateTimeOffset.MinValue;

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
                IBlockchainService blockchainService = scope.ServiceProvider.GetService<IBlockchainService>();
                IStatisticsService statisticsService = scope.ServiceProvider.GetService<IStatisticsService>();

                await CheckNetworkHealthAsync(mapper, blockchainService, statisticsService, cancellationToken);

                if (_lastPeerSnapshot == DateTimeOffset.MinValue ||
                    DateTimeOffset.UtcNow - _lastPeerSnapshot > TimeSpan.FromMinutes(60))
                {
                    await Snapshot(statisticsService, cancellationToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
        }

        private async Task Snapshot(IStatisticsService statisticsService, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Taking snapshot of recent peers");

            await statisticsService.CreateRecentPeerSnapshotAsync(cancellationToken: cancellationToken);
            _lastPeerSnapshot = DateTimeOffset.UtcNow;
        }

        private async Task CheckNetworkHealthAsync(IMapper mapper, IBlockchainService blockchainService,
            IStatisticsService statisticsService, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Retrieving latest network health snapshot");

            IReadOnlyCollection<PeerItem> peers = await blockchainService.GetPeersAsync(cancellationToken);
            if (!peers.Any())
            {
                return;
            }

            await statisticsService.ProcessPeersAsync(mapper.Map<IEnumerable<Peer>>(peers), cancellationToken);
        }
    }
}
