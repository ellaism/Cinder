using System;
using System.Collections.Generic;
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
        private readonly uint _maxBlock = 1000;
        private readonly IMediator _mediator;
        private readonly uint _startBlock = 1;
        private readonly List<GetBlockWithTransactions.Model> _blocks = new List<GetBlockWithTransactions.Model>();
        private uint _currentBlock = 1;

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
                catch (LoggedException) { }
                catch (Exception e)
                {
                    _logger.LogError(e, "{Class} -> {Method}", nameof(BlockchainIndexer), nameof(RunAsync));
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

            GetBlockWithTransactions.Model test =
                await _mediator.Send(new GetBlockWithTransactions.Query {BlockNumber = _currentBlock}, cancellationToken);
            _blocks.Add(test);

            _currentBlock++;
        }
    }
}
