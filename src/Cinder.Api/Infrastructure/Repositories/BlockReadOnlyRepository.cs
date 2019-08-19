using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class BlockReadOnlyRepository : ReadOnlyRepository<CinderBlock>, IBlockReadOnlyRepository
    {
        public BlockReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Blocks) { }

        public async Task<IReadOnlyCollection<CinderBlock>> GetRecentBlocks(int? limit = null,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(FilterDefinition<CinderBlock>.Empty)
                .Limit(limit ?? 10)
                .Sort(new SortDefinitionBuilder<CinderBlock>().Descending(block => block.BlockNumber))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CinderBlock> GetBlockByHash(string hash, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderBlock>.Filter.Eq(document => document.Hash, hash))
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CinderBlock> GetBlockByNumber(ulong number, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderBlock>.Filter.Eq(document => document.BlockNumber, number.ToString()))
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
