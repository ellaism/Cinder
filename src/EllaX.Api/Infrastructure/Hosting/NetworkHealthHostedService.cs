using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class NetworkHealthHostedService : IHostedService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly ILogger<NetworkHealthHostedService> _logger;
        private readonly IStatisticsService _statisticsService;
        private DateTimeOffset _lastPeerCountSnapshot = DateTimeOffset.MinValue;

        public NetworkHealthHostedService(IBlockchainService blockchainService, IStatisticsService statisticsService,
            ILogger<NetworkHealthHostedService> logger)
        {
            _blockchainService = blockchainService;
            _statisticsService = statisticsService;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Network health hosted service is starting");
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await CheckNetworkHealthAsync(cancellationToken);

                if (_lastPeerCountSnapshot == DateTimeOffset.MinValue ||
                    DateTimeOffset.UtcNow - _lastPeerCountSnapshot > TimeSpan.FromMinutes(60))
                {
                    await Snapshot(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Network health hosted service is stopping");

            return Task.CompletedTask;
        }

        private async Task Snapshot(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Taking snapshot of recent peers");

            await _statisticsService.SnapshotRecentPeerCountAsync();
            _lastPeerCountSnapshot = DateTimeOffset.UtcNow;
        }

        private async Task CheckNetworkHealthAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Retrieving latest network health snapshot");

            await _blockchainService.GetHealthAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(120), cancellationToken);
        }
    }
}
