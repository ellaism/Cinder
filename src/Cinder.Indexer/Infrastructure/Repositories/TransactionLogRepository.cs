using System.Numerics;
using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Indexer.Infrastructure.Repositories
{
    public class TransactionLogRepository : IndexerRepositoryBase<CinderTransactionLog>, ITransactionLogRepository
    {
        public TransactionLogRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.TransactionLogs) { }

        public async Task UpsertAsync(FilterLogVO log)
        {
            await UpsertDocumentAsync(log.MapToStorageEntityForUpsert<CinderTransactionLog>());
        }

        public async Task<ITransactionLogView> FindByTransactionHashAndLogIndexAsync(string hash, BigInteger logIndex)
        {
            FilterDefinition<CinderTransactionLog> filter = CreateDocumentFilter(new CinderTransactionLog
            {
                TransactionHash = hash, LogIndex = logIndex.ToString()
            });
            CinderTransactionLog response = await Collection.Find(filter).SingleOrDefaultAsync();

            return response;
        }
    }
}
