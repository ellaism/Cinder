using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface IContractRepository
    {
        Task FillCache();
        Task UpsertAsync(string contractAddress, string code, Transaction transaction);
        Task<bool> ExistsAsync(string contractAddress);

        Task<IContractView> FindByAddressAsync(string contractAddress);
        bool IsCached(string contractAddress);
    }
}
