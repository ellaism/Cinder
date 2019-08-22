using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Clients
{
    public interface IApiClient
    {
        Task<AddressDto> GetAddressByHash(string hash);
        Task<BlockDto> GetBlockByHash(string hash);
        Task<BlockDto> GetBlockByNumber(string number);
        Task<IEnumerable<RecentBlockDto>> GetRecentBlocks(int? limit = null);
        Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions(int? limit = null);
        Task<IEnumerable<TransactionDto>> GetTransactionsByBlockHash(string blockHash);
    }
}
