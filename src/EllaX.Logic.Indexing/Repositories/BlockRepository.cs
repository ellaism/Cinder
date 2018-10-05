using System;
using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Logic.Indexing.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly Repository _repository;

        public BlockRepository(Repository repository)
        {
            _repository = repository;
        }

        public Task UpsertBlockAsync(BlockWithTransactionHashes source)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetMaxBlockNumber()
        {
            throw new NotImplementedException();
        }

        public Task<IBlockView> FindByBlockNumberAsync(HexBigInteger blockNumber)
        {
            return Task.FromResult(
                _repository.SingleOrDefault<IBlockView>(block => block.BlockNumber == blockNumber.Value.ToString()));
        }
    }
}
