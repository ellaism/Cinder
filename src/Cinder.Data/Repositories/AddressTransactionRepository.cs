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

        public async Task<IPage<CinderAddressTransaction>> GetTransactionsByAddressHash(string addressHash, int? page = null,
            int? size = null, SortOrder sort = SortOrder.Ascending, CancellationToken cancellationToken = default)
        {
            IFindFluent<CinderAddressTransaction, CinderAddressTransaction> query =
                Collection.Find(CreateDocumentFilter(new CinderAddressTransaction {Address = addressHash}));
            long total = await query.CountDocumentsAsync(cancellationToken).ConfigureAwait(false);
            query = query.Skip(((page ?? 1) - 1) * (size ?? 10)).Limit(size ?? 10);

            switch (sort)
            {
                case SortOrder.Ascending:
                    query = query.SortBy(addressTransaction => addressTransaction.BlockNumber);
                    break;
                case SortOrder.Descending:
                    query = query.SortByDescending(addressTransaction => addressTransaction.BlockNumber);
                    break;
            }

            List<CinderAddressTransaction> transactions = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedEnumerable<CinderAddressTransaction>(transactions, (int) total, page ?? 1, size ?? 10);
        }
    }
}
