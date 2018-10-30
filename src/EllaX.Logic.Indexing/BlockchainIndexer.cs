using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Exceptions;
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

                try
                {
                    await DoWorkAsync(cancellationToken);
                }
                catch (LoggedException) { }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(StatisticsIndexer)} -> {nameof(RunAsync)}");
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
