using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Data.Repositories
{
    public class AddressTransactionRepository : RepositoryBase<CinderAddressTransaction>, IAddressTransactionRepository
    {
        public AddressTransactionRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.AddressTransactions) { }

        public async Task UpsertAsync(TransactionReceiptVO transactionReceiptVO, string address, string error = null,
            string newContractAddress = null)
        {
            await UpsertDocumentAsync(transactionReceiptVO.MapToStorageEntityForUpsert<CinderAddressTransaction>(address))
                .ConfigureAwait(false);
        }

        public async Task<IAddressTransactionView> FindAsync(string address, HexBigInteger blockNumber, string transactionHash)
        {
            address = address.ToLowerInvariant();
            transactionHash = transactionHash.ToLowerInvariant();
            FilterDefinition<CinderAddressTransaction> filter = CreateDocumentFilter(new CinderAddressTransaction
            {
                Address = address, Hash = transactionHash, BlockNumber = blockNumber.Value.ToString()
            });
            CinderAddressTransaction response = await Collection.Find(filter).SingleOrDefaultAsync().ConfigureAwait(false);

            return response;
        }

        public async Task<IEnumerable<string>> GetUniqueAddresses(CancellationToken cancellationToken)
        {
            using IAsyncCursor<string> t = await Collection
                .DistinctAsync(field => field.Address, new BsonDocument(), cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            List<string> records = new List<string>();
            while (await t.MoveNextAsync(cancellationToken).ConfigureAwait(false)) records.AddRange(t.Current);

            return records;
        }

        public async Task<IEnumerable<string>> GetTransactionHashesByAddressHash(string addressHash, int? page = null,
            int? size = null, SortOrder sort = SortOrder.Ascending, CancellationToken cancellationToken = default)
        {
            page ??= 1;
            size ??= 10;

            IFindFluent<CinderAddressTransaction, CinderAddressTransaction> query = AddressHashBaseQuery(addressHash);
            query = query.Skip((page.Value - 1) * size.Value).Limit(size.Value);

            switch (sort)
            {
                case SortOrder.Ascending:
                    query = query.SortBy(transaction => transaction.BlockNumber);
                    break;
                case SortOrder.Descending:
                    query = query.SortByDescending(transaction => transaction.BlockNumber);
                    break;
            }

            List<string> transactions =
                await query.Project(document => document.Hash).ToListAsync(cancellationToken).ConfigureAwait(false);

            return transactions;
        }

        public async Task<ulong> GetTransactionCountByAddressHash(string addressHash,
            CancellationToken cancellationToken = default)
        {
            IFindFluent<CinderAddressTransaction, CinderAddressTransaction> query = AddressHashBaseQuery(addressHash);
            long total = await query.CountDocumentsAsync(cancellationToken).ConfigureAwait(false);

            return (ulong) total;
        }

        private IFindFluent<CinderAddressTransaction, CinderAddressTransaction> AddressHashBaseQuery(string addressHash)
        {
            addressHash = addressHash.ToLowerInvariant();

            return Collection.Find(Builders<CinderAddressTransaction>.Filter.Eq(transaction => transaction.Address, addressHash));
        }
    }
}
