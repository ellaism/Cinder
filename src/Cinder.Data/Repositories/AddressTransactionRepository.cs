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
            while (await t.MoveNextAsync(cancellationToken).ConfigureAwait(false))
            {
                records.AddRange(t.Current);
            }

            return records;
        }
    }
}
