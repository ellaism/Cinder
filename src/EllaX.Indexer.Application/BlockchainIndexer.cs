using System;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Exceptions;
using EllaX.Indexer.Application.Features.Blockchain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Indexer.Application
{
    public class BlockchainIndexer : IIndexer
    {
        private readonly ILogger<BlockchainIndexer> _logger;
        private readonly ulong _maxBlock = 3023282;
        private readonly IMediator _mediator;
        private ulong _currentBlock = 3023282;

        public BlockchainIndexer(ILogger<BlockchainIndexer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
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
                catch (Exception e)
                {
                    if (!(e is LoggedException))
                    {
                        _logger.LogError(e, "{Class} -> {Method}", nameof(BlockchainIndexer), nameof(RunAsync));
                    }

                    await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken = default)
        {
            if (_currentBlock > _maxBlock)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                return;
            }

            await _mediator.Send(new IndexBlockWithTransactions.Command {BlockNumber = _currentBlock}, cancellationToken);

            _currentBlock++;
        }
    }
}
