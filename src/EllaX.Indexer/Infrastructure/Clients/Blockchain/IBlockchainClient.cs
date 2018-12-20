using System.Threading;
using System.Threading.Tasks;
using EllaX.Indexer.Infrastructure.Clients.Responses;
using EllaX.Indexer.Infrastructure.Clients.Responses.Eth;

namespace EllaX.Indexer.Infrastructure.Clients.Blockchain
{
    public interface IBlockchainClient
    {
        Task<Response<ulong>> GetHeight(CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(string blockHash, CancellationToken cancellationToken = default);
        Task<Response<BlockResult>> GetBlock(ulong blockNumber, CancellationToken cancellationToken = default);
    }
}
