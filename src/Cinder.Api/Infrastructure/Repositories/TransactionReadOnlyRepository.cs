using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class TransactionReadOnlyRepository : ReadOnlyRepository<CinderTransaction>, ITransactionReadOnlyRepository
    {
        public TransactionReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Transactions) { }

        public async Task<IReadOnlyCollection<CinderTransaction>> GetRecentTransactions(int limit = 10,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(FilterDefinition<CinderTransaction>.Empty)
                .Limit(limit)
                .Sort(new SortDefinitionBuilder<CinderTransaction>().Descending(block => block.TimeStamp))
                .ToListAsync(cancellationToken)
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
