using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class TransactionReadOnlyRepository : ReadOnlyRepository<CinderTransaction>, ITransactionReadOnlyRepository
    {
        public TransactionReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Transactions) { }

        public async Task<IPage<CinderTransaction>> GetTransactions(int? page = null, int? size = null,
            SortOrder sort = SortOrder.Ascending, CancellationToken cancellationToken = default)
        {
            long total = await Collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            IFindFluent<CinderTransaction, CinderTransaction> query = Collection.Find(FilterDefinition<CinderTransaction>.Empty)
                .Skip(((page ?? 1) - 1) * (size ?? 10))
                .Limit(size ?? 10);

            switch (sort)
            {
                case SortOrder.Ascending:
                    query = query.SortBy(transaction => transaction.TimeStamp);
                    break;
                case SortOrder.Descending:
                    query = query.SortByDescending(transaction => transaction.TimeStamp);
                    break;
            }

            List<CinderTransaction> transactions = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedEnumerable<CinderTransaction>(transactions, (int) total, page ?? 1, size ?? 10);
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

        public async Task<string> GetTransactionHashIfExists(string hash, CancellationToken cancellationToken = default)
        {
            var result = await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.Hash, hash))
                .Project(transaction => new {transaction.Hash})
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.Hash;
        }
    }
}
