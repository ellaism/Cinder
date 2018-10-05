using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing.Transactions
{
    public class TransactionProcessor : ITransactionProcessor
    {
        private readonly IContractCreationTransactionProcessor _contractCreationTransactionProcessor;
        private readonly IValueTransactionProcessor _valueTransactionProcessor;

        public TransactionProcessor(Web3 web3, IContractTransactionProcessor contractTransactionProcessor,
            IValueTransactionProcessor valueTransactionProcessor,
            IContractCreationTransactionProcessor contractCreationTransactionProcessor)
        {
            ContractTransactionProcessor = contractTransactionProcessor;
            _valueTransactionProcessor = valueTransactionProcessor;
            _contractCreationTransactionProcessor = contractCreationTransactionProcessor;
            Web3 = web3;
        }

        protected Web3 Web3 { get; }

        public virtual async Task ProcessTransactionAsync(string transactionHash, BlockWithTransactionHashes block)
        {
            Transaction transactionSource = await GetTransaction(transactionHash).ConfigureAwait(false);
            TransactionReceipt transactionReceipt = await GetTransactionReceipt(transactionHash).ConfigureAwait(false);

            if (_contractCreationTransactionProcessor.IsTransactionForContractCreation(transactionSource,
                transactionReceipt))
            {
                if (EnabledContractCreationProcessing)
                {
                    await _contractCreationTransactionProcessor
                        .ProcessTransactionAsync(transactionSource, transactionReceipt, block.Timestamp)
                        .ConfigureAwait(false);
                }
            }
            else
            {
                if (await ContractTransactionProcessor.IsTransactionForContractAsync(transactionSource))
                {
                    if (EnabledContractProcessing)
                    {
                        await ContractTransactionProcessor
                            .ProcessTransactionAsync(transactionSource, transactionReceipt, block.Timestamp)
                            .ConfigureAwait(false);
                    }
                }
                else if (EnabledValueProcessing)
                {
                    await _valueTransactionProcessor
                        .ProcessTransactionAsync(transactionSource, transactionReceipt, block.Timestamp)
                        .ConfigureAwait(false);
                }
            }
        }

        public bool EnabledContractCreationProcessing { get; set; } = true;
        public bool EnabledContractProcessing { get; set; } = true;
        public bool EnabledValueProcessing { get; set; } = true;
        public IContractTransactionProcessor ContractTransactionProcessor { get; }

        public async Task<Transaction> GetTransaction(string txnHash)
        {
            return await Web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txnHash).ConfigureAwait(false);
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string txnHash)
        {
            return await Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash).ConfigureAwait(false);
        }
    }
}
