using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class BlockReadOnlyRepository : ReadOnlyRepository<CinderBlock>, IBlockReadOnlyRepository
    {
        public BlockReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Blocks) { }

        public async Task<IPage<CinderBlock>> GetBlocks(int? page = null, int? size = null, SortOrder sort = SortOrder.Ascending,
            CancellationToken cancellationToken = default)
        {
            long total = await Collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            IFindFluent<CinderBlock, CinderBlock> query = Collection.Find(FilterDefinition<CinderBlock>.Empty)
                .Skip(((page ?? 1) - 1) * (size ?? 10))
                .Limit(size ?? 10);

            switch (sort)
            {
                case SortOrder.Ascending:
                    query = query.SortBy(block => block.Id);
                    break;
                case SortOrder.Descending:
                    query = query.SortByDescending(block => block.Id);
                    break;
            }

            List<CinderBlock> blocks = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedEnumerable<CinderBlock>(blocks, (int) total, page ?? 1, size ?? 10);
        }

        public async Task<CinderBlock> GetBlockByHash(string hash, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderBlock>.Filter.Eq(document => document.Hash, hash))
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> GetBlockHashIfExists(string hash, CancellationToken cancellationToken = default)
        {
            var result = await Collection.Find(Builders<CinderBlock>.Filter.Eq(document => document.Hash, hash))
                .Project(block => new {block.Hash})
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.Hash;
        }

        public async Task<CinderBlock> GetBlockByNumber(ulong number, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderBlock>.Filter.Eq(document => document.BlockNumber, number.ToString()))
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> GetBlockNumberIfExists(ulong number, CancellationToken cancellationToken = default)
        {
            var result = await Collection
                .Find(Builders<CinderBlock>.Filter.Eq(document => document.BlockNumber, number.ToString()))
                .Project(block => new {block.BlockNumber})
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.BlockNumber;
        }
    }
}
