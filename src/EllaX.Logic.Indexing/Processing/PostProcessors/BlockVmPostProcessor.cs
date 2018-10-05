using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;
using EllaX.Logic.Indexing.Transactions;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing.Processing.PostProcessors
{
    public class BlockVmPostProcessor : BlockPostProcessor
    {
        public BlockVmPostProcessor(Web3 web3, IBlockRepository blockRepository,
            ITransactionProcessor transactionProcessor) : base(web3, blockRepository, transactionProcessor) { }

        public override async Task ProcessBlockAsync(long blockNumber)
        {
            TransactionProcessor.EnabledValueProcessing = false;
            TransactionProcessor.EnabledContractCreationProcessing = false;
            TransactionProcessor.EnabledContractProcessing = true;
            TransactionProcessor.ContractTransactionProcessor.EnabledVmProcessing = true;
            await base.ProcessBlockAsync(blockNumber);
        }
    }
}
