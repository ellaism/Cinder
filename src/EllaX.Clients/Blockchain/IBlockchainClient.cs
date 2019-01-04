using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;

namespace EllaX.Clients.Blockchain
{
    public interface IBlockchainClient
    {
        Task<Response<BigInteger>> GetLatestBlockAsync(CancellationToken cancellationToken = default);

        Task<Response<BlockResult>> GetBlockWithTransactionsAsync(BigInteger blockNumber,
            CancellationToken cancellationToken = default);
    }
}
