using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface ITransactionRepository
    {
        Task UpsertAsync(string contractAddress, string code, Transaction transaction,
            TransactionReceipt transactionReceipt, bool failedCreatingContract, HexBigInteger blockTimestamp);

        Task UpsertAsync(Transaction transaction, TransactionReceipt receipt, bool failed, HexBigInteger timeStamp,
            bool hasVmStack = false, string error = null);

        Task<ITransactionView> FindByBlockNumberAndHashAsync(HexBigInteger blockNumber, string hash);
    }
}
