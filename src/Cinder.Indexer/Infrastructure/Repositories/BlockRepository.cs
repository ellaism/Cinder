using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.Hex.HexTypes;
using Block = Nethereum.RPC.Eth.DTOs.Block;

namespace Cinder.Indexer.Infrastructure.Repositories
{
    public class BlockRepository : IndexerRepositoryBase<CinderBlock>, IBlockRepository
    {
        public BlockRepository(IMongoClient client, string databaseName) : base(client, databaseName, CollectionName.Blocks) { }

        public async Task UpsertBlockAsync(Block source)
        {
            CinderBlock document = source.MapToStorageEntityForUpsert<CinderBlock>();
            document.Sha3Uncles = source.Sha3Uncles;
            document.Uncles = source.Uncles;
            document.UncleCount = source.Uncles.Length;
            await UpsertDocumentAsync(document).ConfigureAwait(false);
        }

        public async Task<IBlockView> FindByBlockNumberAsync(HexBigInteger blockNumber)
        {
            FilterDefinition<CinderBlock> filter =
                CreateDocumentFilter(new CinderBlock {BlockNumber = blockNumber.Value.ToString()});
            CinderBlock response = await Collection.Find(filter).SingleOrDefaultAsync().ConfigureAwait(false);

            return response;
        }
    }
}
