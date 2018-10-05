using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EllaX.Logic.Indexing.Exceptions;
using EllaX.Logic.Indexing.Repositories;
using EllaX.Logic.Indexing.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing
{
    public class BlockProcessor : IBlockProcessor
    {
        private readonly IBlockRepository _blockRepository;

        public BlockProcessor(Web3 web3, IBlockRepository blockRepository, ITransactionProcessor transactionProcessor)
        {
            _blockRepository = blockRepository;
            TransactionProcessor = transactionProcessor;
            Web3 = web3;
        }

        public static bool ProcessTransactionsInParallel { get; set; } = true;

        protected Web3 Web3 { get; set; }
        protected ITransactionProcessor TransactionProcessor { get; set; }

        public virtual async Task ProcessBlockAsync(long blockNumber)
        {
            BlockWithTransactionHashes block = await GetBlockWithTransactionHashesAsync(blockNumber);

            if (block == null)
            {
                throw new BlockNotFoundException(blockNumber);
            }

            await _blockRepository.UpsertBlockAsync(block);

            if (ProcessTransactionsInParallel)
            {
                await ProcessTransactionsMultiThreaded(block);
            }
            else
            {
                await ProcessTransactions(block);
            }
        }

        protected async Task ProcessTransactions(BlockWithTransactionHashes block)
        {
            foreach (string txnHash in block.TransactionHashes)
            {
                await TransactionProcessor.ProcessTransactionAsync(txnHash, block);
            }
        }

        protected async Task ProcessTransactionsMultiThreaded(BlockWithTransactionHashes block)
        {
            List<Task> tasks = new List<Task>(block.TransactionHashes.Length);
            tasks.AddRange(block.TransactionHashes.Select(transactionHash =>
                TransactionProcessor.ProcessTransactionAsync(transactionHash, block)));

            await Task.WhenAll(tasks);
        }

        protected async Task<BlockWithTransactionHashes> GetBlockWithTransactionHashesAsync(long blockNumber)
        {
            BlockWithTransactionHashes block = await Web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber
                .SendRequestAsync(new HexBigInteger(blockNumber)).ConfigureAwait(false);

            return block;
        }
    }
}
