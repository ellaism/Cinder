using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class IndexerHostedService : IHostedService
    {
        private readonly ILogger<IndexerHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IndexerHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<IndexerHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexer hosted service is starting");
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IIndexer indexer = scope.ServiceProvider.GetService<IIndexer>();
                await DoWork(indexer, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexer hosted service is stopping");

            return Task.CompletedTask;
        }

        private async Task DoWork(IIndexer indexer, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                // delay startup
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                // run indexer
                await indexer.RunAsync(cancellationToken);
            }, cancellationToken);
        }
    }
}
