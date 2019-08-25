using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Paging;

namespace Cinder.UI.Infrastructure.Clients
{
    public interface IApiClient
    {
        Task<AddressDto> GetAddressByHash(string hash);
        Task<BlockDto> GetBlockByHash(string hash);
        Task<BlockDto> GetBlockByNumber(string number);
        Task<IPage<BlockDto>> GetBlocks(int? page, int? size);
        Task<TransactionDto> GetTransactionByHash(string hash);
        Task<IPage<TransactionDto>> GetTransactions(int? page, int? size);
        Task<IEnumerable<TransactionDto>> GetTransactionsByBlockHash(string blockHash);
    }
}
