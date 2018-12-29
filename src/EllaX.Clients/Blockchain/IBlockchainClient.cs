using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;

namespace EllaX.Clients.Blockchain
{
    public interface IBlockchainClient
    {
        Task<Response<ulong>> GetHeightAsync(CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlockAsync(string blockHash, CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlockAsync(ulong blockNumber, CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlockAsync(BlockType earliest, CancellationToken cancellationToken = default);
    }
}
