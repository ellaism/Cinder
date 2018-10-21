using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Clients.Responses;
using EllaX.Logic.Clients.Responses.Eth;

namespace EllaX.Logic.Clients
{
    public interface IBlockchainClient
    {
        Task<Response<ulong>> GetHeight(CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(string blockHash, CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(ulong blockNumber, CancellationToken cancellationToken = default);
    }
}
