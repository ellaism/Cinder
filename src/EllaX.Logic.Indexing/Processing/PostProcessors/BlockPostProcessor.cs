using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;
using EllaX.Logic.Indexing.Transactions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing.Processing.PostProcessors
{
    public class BlockPostProcessor : BlockProcessor
    {
        public BlockPostProcessor(Web3 web3, IBlockRepository blockRepository,
            ITransactionProcessor transactionProcessor) : base(web3, blockRepository, transactionProcessor) { }

        public override async Task ProcessBlockAsync(long blockNumber)
        {
            BlockWithTransactionHashes block = await GetBlockWithTransactionHashesAsync(blockNumber);
            //no need to save the block again
            await ProcessTransactions(block);
        }
    }
}
