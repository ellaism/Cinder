using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Indexing
{
    public class BlockchainIndexer : IIndexer
    {
        private readonly ILogger<BlockchainIndexer> _logger;

        public BlockchainIndexer(ILogger<BlockchainIndexer> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Blockchain indexer starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Blockchain indexer polling");
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
