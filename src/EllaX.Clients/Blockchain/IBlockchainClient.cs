using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;

namespace EllaX.Clients.Blockchain
{
    public interface IBlockchainClient
    {
        Task<Response<ulong>> GetHeight(CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(string blockHash, CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(ulong blockNumber, CancellationToken cancellationToken = default);
    }
}
