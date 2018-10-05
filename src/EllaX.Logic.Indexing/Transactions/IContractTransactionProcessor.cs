using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Transactions
{
    public interface IContractTransactionProcessor
    {
        bool EnabledVmProcessing { get; set; }
        Task<bool> IsTransactionForContractAsync(Transaction transaction);

        Task ProcessTransactionAsync(Transaction transaction, TransactionReceipt transactionReceipt,
            HexBigInteger blockTimestamp);
    }
}
