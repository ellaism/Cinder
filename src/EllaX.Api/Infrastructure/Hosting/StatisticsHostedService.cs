using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Services;
using EllaX.Logic.Services.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class StatisticsHostedService : IHostedService
    {
        private readonly ILogger<StatisticsHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private DateTimeOffset _lastPeerCountSnapshot = DateTimeOffset.MinValue;

        public StatisticsHostedService(IServiceScopeFactory serviceScopeFactory,
            ILogger<StatisticsHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Statistics hosted service is starting");
            await DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Statistics health hosted service is stopping");

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                // delay startup
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
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

                    await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
                }
            }, cancellationToken);
        }

        private async Task Snapshot(IStatisticsService statisticsService, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Taking snapshot of recent peers");

            await statisticsService.SnapshotRecentPeerCountAsync();
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
