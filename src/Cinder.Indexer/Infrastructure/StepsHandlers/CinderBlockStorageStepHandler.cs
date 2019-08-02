using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nethereum.BlockchainProcessing.BlockStorage.BlockStorageStepsHandlers;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Indexer.Infrastructure.StepsHandlers
{
    public class CinderBlockStorageStepHandler : BlockStorageStepHandler
    {
        private readonly ILogger<CinderBlockStorageStepHandler> _logger;

        public CinderBlockStorageStepHandler(ILogger<CinderBlockStorageStepHandler> logger, IBlockRepository blockRepository) :
            base(blockRepository)
        {
            _logger = logger;
        }

        protected override async Task ExecuteInternalAsync(BlockWithTransactions value)
        {
            _logger.LogInformation("Processing block {Block}", value.Number.ToUlong());
            await base.ExecuteInternalAsync(value).ConfigureAwait(false);
            //await _bus.Publish(BlockEvent.Create(value.Number.ToUlong()));
        }
    }
}
