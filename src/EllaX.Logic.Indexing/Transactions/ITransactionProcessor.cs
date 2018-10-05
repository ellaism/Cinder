using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Transactions
{
    public interface ITransactionProcessor
    {
        bool EnabledContractCreationProcessing { get; set; }
        bool EnabledContractProcessing { get; set; }
        bool EnabledValueProcessing { get; set; }
        IContractTransactionProcessor ContractTransactionProcessor { get; }
        Task ProcessTransactionAsync(string transactionHash, BlockWithTransactionHashes block);
    }
}
