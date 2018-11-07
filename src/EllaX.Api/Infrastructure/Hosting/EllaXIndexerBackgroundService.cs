using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class EllaXIndexerBackgroundService : BackgroundService
    {
        private readonly ILogger<EllaXIndexerBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EllaXIndexerBackgroundService(IServiceScopeFactory serviceScopeFactory,
            ILogger<EllaXIndexerBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                // delay startup
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    IIndexerManager indexerManager = scope.ServiceProvider.GetService<IIndexerManager>();

                    // run indexer manager
                    await indexerManager.RunAsync(stoppingToken);
                }
            }, stoppingToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EllaX Indexer is starting");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EllaX Indexer is stopping");

            return base.StopAsync(cancellationToken);
        }
    }
}
