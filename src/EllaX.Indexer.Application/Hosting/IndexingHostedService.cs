using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EllaX.Indexer.Application.Hosting
{
    public class IndexingHostedService : BackgroundService
    {
        private readonly IIndexerManager _indexManager;

        public IndexingHostedService(IIndexerManager indexManager)
        {
            _indexManager = indexManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _indexManager.RunAsync(stoppingToken);
        }
    }
}
