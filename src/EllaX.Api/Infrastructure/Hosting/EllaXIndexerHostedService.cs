using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class EllaXIndexerHostedService : IHostedService
    {
        private readonly ILogger<EllaXIndexerHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EllaXIndexerHostedService(IServiceScopeFactory serviceScopeFactory,
            ILogger<EllaXIndexerHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexer hosted service is starting");
            await DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexer hosted service is stopping");

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                // delay startup
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    IIndexerManager indexerManager = scope.ServiceProvider.GetService<IIndexerManager>();

                    // run indexer manager
                    await indexerManager.RunAsync(cancellationToken);
                }
            }, cancellationToken);
        }
    }
}
