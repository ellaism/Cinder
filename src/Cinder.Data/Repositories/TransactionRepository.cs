using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Documents;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Data.Repositories
{
    public class TransactionRepository : RepositoryBase<CinderTransaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Transactions) { }

        public async Task<ITransactionView> FindByBlockNumberAndHashAsync(HexBigInteger blockNumber, string hash)
        {
            hash = hash.ToLowerInvariant();
            FilterDefinition<CinderTransaction> filter = CreateDocumentFilter(new CinderTransaction
            {
                BlockNumber = blockNumber.Value.ToString(), Hash = hash
            });
            CinderTransaction response = await Collection.Find(filter).SingleOrDefaultAsync().ConfigureAwait(false);

            return response;
        }

        public async Task UpsertAsync(TransactionReceiptVO transactionReceiptVO, string code, bool failedCreatingContract)
        {
            await UpsertDocumentAsync(
                    transactionReceiptVO.MapToStorageEntityForUpsert<CinderTransaction>(code, failedCreatingContract))
                .ConfigureAwait(false);
        }

        public async Task UpsertAsync(TransactionReceiptVO transactionReceiptVO)
        {
            await UpsertDocumentAsync(transactionReceiptVO.MapToStorageEntityForUpsert<CinderTransaction>())
                .ConfigureAwait(false);
        }

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
            hash = hash.ToLowerInvariant();

            return await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.Hash, hash))
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CinderTransaction>> GetTransactionsByBlockHash(string blockHash,
            CancellationToken cancellationToken = default)
        {
            blockHash = blockHash.ToLowerInvariant();

            return await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.BlockHash, blockHash))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> GetTransactionHashIfExists(string hash, CancellationToken cancellationToken = default)
        {
            hash = hash.ToLowerInvariant();
            var result = await Collection.Find(Builders<CinderTransaction>.Filter.Eq(document => document.Hash, hash))
                .Project(transaction => new {transaction.Hash})
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.Hash;
        }

        public async Task<IPage<CinderTransaction>> GetTransactionsByAddressHash(string addressHash, int? page = null,
            int? size = null, bool? limited = false, SortOrder sort = SortOrder.Ascending,
            CancellationToken cancellationToken = default)
        {
            page ??= 1;
            size ??= 10;
            limited ??= false;

            IFindFluent<CinderTransaction, CinderTransaction> query = AddressHashBaseQuery(addressHash);
            // TODO 20190915 Additional performance improvements are needed here
            long total = await query.Limit(limited.Value ? page.Value * size.Value + 1 : 5000)
                .CountDocumentsAsync(cancellationToken)
                .ConfigureAwait(false);

            switch (sort)
            {
                case SortOrder.Ascending:
                    query = query.SortBy(transaction => transaction.Id);
                    break;
                case SortOrder.Descending:
                    query = query.SortByDescending(transaction => transaction.Id);
                    break;
            }

            query = query.Skip((page.Value - 1) * size.Value).Limit(size.Value);
            List<CinderTransaction> transactions = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedEnumerable<CinderTransaction>(transactions, (int) total, page.Value, size.Value);
        }

        public async Task<ulong> GetTransactionCountByAddressHash(string addressHash,
            CancellationToken cancellationToken = default)
        {
            IFindFluent<CinderTransaction, CinderTransaction> query = AddressHashBaseQuery(addressHash);
            long total = await query.CountDocumentsAsync(cancellationToken).ConfigureAwait(false);

            return (ulong) total;
        }

        private IFindFluent<CinderTransaction, CinderTransaction> AddressHashBaseQuery(string addressHash)
        {
            addressHash = addressHash.ToLowerInvariant();

            return Collection.Find(Builders<CinderTransaction>.Filter.Or(
                Builders<CinderTransaction>.Filter.Eq(transaction => transaction.AddressTo, addressHash),
                Builders<CinderTransaction>.Filter.Eq(transaction => transaction.AddressFrom, addressHash)));
        }
    }
}
