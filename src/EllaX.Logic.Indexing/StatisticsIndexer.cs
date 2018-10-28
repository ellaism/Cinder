using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Statistics;
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
                IBlockchainService blockchainService = scope.ServiceProvider.GetService<IBlockchainService>();
                IStatisticsService statisticsService = scope.ServiceProvider.GetService<IStatisticsService>();

                await CheckNetworkHealthAsync(blockchainService, cancellationToken);

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

        private async Task CheckNetworkHealthAsync(IBlockchainService blockchainService,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Retrieving latest network health snapshot");

            await blockchainService.GetHealthAsync(cancellationToken);
        }
    }
}
