using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;

namespace EllaX.Logic.Indexing.Transactions
{
    public class ContractTransactionProcessor : IContractTransactionProcessor
    {
        private readonly IAddressTransactionRepository _addressTransactionRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionVmStackRepository _transactionVmStackRepository;

        public ContractTransactionProcessor(Web3 web3, IContractRepository contractRepository,
            ITransactionRepository transactionRepository, IAddressTransactionRepository addressTransactionRepository,
            ITransactionVmStackRepository transactionVmStackRepository,
            ITransactionLogRepository transactionLogRepository)
        {
            Web3 = web3;
            _contractRepository = contractRepository;
            _transactionRepository = transactionRepository;
            _addressTransactionRepository = addressTransactionRepository;
            _transactionVmStackRepository = transactionVmStackRepository;
            _transactionLogRepository = transactionLogRepository;
            // VmStackErrorChecker = new VmStackErrorChecker();
        }

        //protected VmStackErrorChecker VmStackErrorChecker { get; set; }
        protected Web3 Web3 { get; set; }

        public async Task<bool> IsTransactionForContractAsync(Transaction transaction)
        {
            if (transaction.To == null)
            {
                return false;
            }

            return await _contractRepository.ExistsAsync(transaction.To).ConfigureAwait(false);
        }

        public bool EnabledVmProcessing { get; set; } = true;

        public async Task ProcessTransactionAsync(Transaction transaction, TransactionReceipt transactionReceipt,
            HexBigInteger blockTimestamp)
        {
            string transactionHash = transaction.TransactionHash;
            bool hasStackTrace = false;
            JObject stackTrace = null;
            string error = string.Empty;
            bool hasError = false;

            if (EnabledVmProcessing)
            {
                try
                {
                    stackTrace = await GetTransactionVmStack(transactionHash).ConfigureAwait(false);
                }
                catch
                {
                    if (transaction.Gas == transactionReceipt.GasUsed)
                    {
                        hasError = true;
                    }
                }

                if (stackTrace != null)
                {
                    //error = VmStackErrorChecker.GetError(stackTrace);
                    hasError = !string.IsNullOrEmpty(error);
                    hasStackTrace = true;
                    await _transactionVmStackRepository.UpsertAsync(transactionHash, transaction.To, stackTrace);
                }
            }

            JArray logs = transactionReceipt.Logs;

            await _transactionRepository.UpsertAsync(transaction, transactionReceipt, hasError, blockTimestamp,
                hasStackTrace, error);

            await _addressTransactionRepository.UpsertAsync(transaction, transactionReceipt, hasError, blockTimestamp,
                transaction.To, error, hasStackTrace);

            List<string> addressesAdded = new List<string> {transaction.To};

            for (int i = 0; i < logs.Count; i++)
            {
                if (!(logs[i] is JObject log))
                {
                    continue;
                }

                string logAddress = log["address"].Value<string>();

                if (!addressesAdded.Exists(x => x == logAddress))
                {
                    addressesAdded.Add(logAddress);

                    await _addressTransactionRepository.UpsertAsync(transaction, transactionReceipt, hasError,
                        blockTimestamp, logAddress, error, hasStackTrace);
                }

                await _transactionLogRepository.UpsertAsync(transactionHash, i, log);
            }
        }

        protected async Task<JObject> GetTransactionVmStack(string transactionHash)
        {
            return null;
            //return
            //    await
            //        Web3.DebugGeth.TraceTransaction.SendRequestAsync(transactionHash,
            //            new TraceTransactionOptions {DisableMemory = true, DisableStorage = true, DisableStack = true});
        }
    }
}
