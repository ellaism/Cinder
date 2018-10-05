using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface IBlockRepository
    {
        Task UpsertBlockAsync(BlockWithTransactionHashes source);
        Task<long> GetMaxBlockNumber();
        Task<IBlockView> FindByBlockNumberAsync(HexBigInteger blockNumber);
    }
}
