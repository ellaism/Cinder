using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Transactions
{
    public interface IValueTransactionProcessor
    {
        Task ProcessTransactionAsync(Transaction transaction, TransactionReceipt transactionReceipt,
            HexBigInteger blockTimestamp);
    }
}
