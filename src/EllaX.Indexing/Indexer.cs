using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EllaX.Indexing
{
    public class Indexer : IIndexer
    {
        private readonly ILogger<Indexer> _logger;

        public Indexer(ILogger<Indexer> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Indexer polling");
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
