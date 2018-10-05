using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing.Transactions
{
    public class ContractCreationTransactionProcessor : IContractCreationTransactionProcessor
    {
        private readonly IAddressTransactionRepository _addressTransactionRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly Web3 _web3;

        public ContractCreationTransactionProcessor(Web3 web3, IContractRepository contractRepository,
            ITransactionRepository transactionRepository, IAddressTransactionRepository addressTransactionRepository)
        {
            _web3 = web3;
            _contractRepository = contractRepository;
            _transactionRepository = transactionRepository;
            _addressTransactionRepository = addressTransactionRepository;
        }

        public async Task ProcessTransactionAsync(Transaction transaction, TransactionReceipt transactionReceipt,
            HexBigInteger blockTimestamp)
        {
            if (!IsTransactionForContractCreation(transaction, transactionReceipt))
            {
                return;
            }

            string contractAddress = GetContractAddress(transactionReceipt);
            string code = await GetCode(contractAddress).ConfigureAwait(false);
            bool failedCreatingContract = HasFailedToCreateContract(code);

            if (!failedCreatingContract)
            {
                await _contractRepository.UpsertAsync(contractAddress, code, transaction).ConfigureAwait(false);
            }

            await _transactionRepository.UpsertAsync(contractAddress, code, transaction, transactionReceipt,
                failedCreatingContract, blockTimestamp);

            await _addressTransactionRepository.UpsertAsync(transaction, transactionReceipt, failedCreatingContract,
                blockTimestamp, null, null, false, contractAddress);
        }

        public bool IsTransactionForContractCreation(Transaction transaction, TransactionReceipt transactionReceipt)
        {
            return IsAddressEmpty(transaction.To) && !string.IsNullOrEmpty(GetContractAddress(transactionReceipt));
        }

        public async Task<string> GetCode(string contractAddress)
        {
            return await _web3.Eth.GetCode.SendRequestAsync(contractAddress).ConfigureAwait(false);
        }

        public bool HasFailedToCreateContract(string code)
        {
            return code == null || code == "0x";
        }

        private bool IsAddressEmpty(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return true;
            }

            return address == "0x0";
        }

        private static string GetContractAddress(TransactionReceipt transactionReceipt)
        {
            return transactionReceipt.ContractAddress;
        }
    }
}
