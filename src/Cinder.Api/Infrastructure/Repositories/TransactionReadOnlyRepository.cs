using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data;
using Cinder.Documents;
using Cinder.Extensions;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class TransactionReadOnlyRepository : ReadOnlyRepository<CinderTransaction>, ITransactionReadOnlyRepository
    {
        public TransactionReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Transactions) { }

        public async Task<IPage<CinderTransaction>> GetTransactions(int? page = null, int? size = null,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(FilterDefinition<CinderTransaction>.Empty)
                .Sort(new SortDefinitionBuilder<CinderTransaction>().Descending(block => block.TimeStamp))
                .ToPageAsync(page ?? 1, size ?? 10, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CinderTransaction> GetTransactionByHash(string hash, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.Hash, hash))
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CinderTransaction>> GetTransactionByBlockHash(string blockHash,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.BlockHash, blockHash))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
