using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data;
using Cinder.Documents;
using Cinder.Extensions;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class BlockReadOnlyRepository : ReadOnlyRepository<CinderBlock>, IBlockReadOnlyRepository
    {
        public BlockReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Blocks) { }

        public async Task<IPage<CinderBlock>> GetBlocks(int? page = null, int? size = null,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(FilterDefinition<CinderBlock>.Empty)
                .Sort(new SortDefinitionBuilder<CinderBlock>().Descending(block => block.BlockNumber))
                .ToPageAsync(page ?? 1, size ?? 10, cancellationToken)
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
