using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Entities;
using Nethereum.Web3;

namespace EllaX.Clients
{
    public interface IBlockchainClient
    {
        Web3 Web3 { get; }
        Task<Block> GetBlockWithTransactionsAsync(ulong blockNumber, CancellationToken cancellationToken = default);
        Task<ulong> GetLatestBlockNumberAsync(CancellationToken cancellationToken = default);
    }
}
