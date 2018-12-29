using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EllaX.Indexing.HostedServices
{
    public class IndexerHostedService : BackgroundService
    {
        private readonly IIndexerManager _indexManager;

        public IndexerHostedService(IIndexerManager indexManager)
        {
            _indexManager = indexManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _indexManager.RunAsync(stoppingToken);
        }
    }
}
