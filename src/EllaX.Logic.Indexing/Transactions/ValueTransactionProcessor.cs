using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Transactions
{
    public class ValueTransactionProcessor : IValueTransactionProcessor
    {
        private readonly IAddressTransactionRepository _addressTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ValueTransactionProcessor(ITransactionRepository transactionRepository,
            IAddressTransactionRepository addressTransactionRepository)
        {
            _transactionRepository = transactionRepository;
            _addressTransactionRepository = addressTransactionRepository;
        }

        public async Task ProcessTransactionAsync(Transaction transaction, TransactionReceipt transactionReceipt,
            HexBigInteger blockTimestamp)
        {
            await _transactionRepository.UpsertAsync(transaction, transactionReceipt, false, blockTimestamp)
                .ConfigureAwait(false);
            await _addressTransactionRepository.UpsertAsync(transaction,
                transactionReceipt, false, blockTimestamp, transaction.To).ConfigureAwait(false);
        }
    }
}
