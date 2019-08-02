using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Indexer.Infrastructure.Repositories
{
    public class TransactionRepository : IndexerRepositoryBase<CinderTransaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Transactions) { }

        public async Task<ITransactionView> FindByBlockNumberAndHashAsync(HexBigInteger blockNumber, string hash)
        {
            FilterDefinition<CinderTransaction> filter = CreateDocumentFilter(new CinderTransaction
            {
                BlockNumber = blockNumber.Value.ToString(), Hash = hash
            });
            CinderTransaction response = await Collection.Find(filter).SingleOrDefaultAsync();

            return response;
        }

        public async Task UpsertAsync(TransactionReceiptVO transactionReceiptVO, string code, bool failedCreatingContract)
        {
            await UpsertDocumentAsync(
                transactionReceiptVO.MapToStorageEntityForUpsert<CinderTransaction>(code, failedCreatingContract));
        }

        public async Task UpsertAsync(TransactionReceiptVO transactionReceiptVO)
        {
            await UpsertDocumentAsync(transactionReceiptVO.MapToStorageEntityForUpsert<CinderTransaction>());
        }
    }
}
