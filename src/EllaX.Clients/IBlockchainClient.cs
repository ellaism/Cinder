using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Entities;
using Nethereum.Web3;

namespace EllaX.Clients
{
    public interface IBlockchainClient
    {
        Web3 Web3 { get; }
        Task<Block> GetBlockWithTransactionsAsync(uint blockNumber, CancellationToken cancellationToken = default);
        Task<uint> GetLatestBlockNumberAsync(CancellationToken cancellationToken = default);
    }
}
