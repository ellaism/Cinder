using System;
using System.Threading.Tasks;
using Cinder.Core.SharedKernel;
using Cinder.Data;
using Cinder.Data.IndexBuilders;
using Cinder.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinder.Indexer.Infrastructure.Repositories
{
    public class IndexerRepositoryFactory : RepositoryFactoryBase, IIndexerRepositoryFactory
    {
        public IndexerRepositoryFactory(string connectionString, string dbTag) : base(connectionString, dbTag) { }

        public IMongoDatabase CreateDbIfNotExists()
        {
            return Client.GetDatabase(DatabaseName);
        }

        public async Task DeleteDatabase()
        {
            await Client.DropDatabaseAsync(DatabaseName).ConfigureAwait(false);
        }

        public async Task CreateCollectionsIfNotExist(IMongoDatabase db, string locale)
        {
            foreach (CollectionName collectionName in (CollectionName[]) Enum.GetValues(typeof(CollectionName)))
            {
                IAsyncCursor<BsonDocument> collections = await db.ListCollectionsAsync(new ListCollectionsOptions
                    {
                        Filter = new BsonDocument("name", collectionName.ToCollectionName())
                    })
                    .ConfigureAwait(false);

                if (!await collections.AnyAsync().ConfigureAwait(false))
                {
                    await db.CreateCollectionAsync(collectionName.ToCollectionName(),
                            new CreateCollectionOptions {Collation = new Collation(locale, numericOrdering: true)})
                        .ConfigureAwait(false);
                }

                IIndexBuilder builder;
                switch (collectionName)
                {
                    case CollectionName.Addresses:
                        builder = new AddressIndexBuilder(db);
                        break;
                    case CollectionName.AddressTransactions:
                        builder = new AddressTransactionIndexBuilder(db);
                        break;
                    case CollectionName.Blocks:
                        builder = new BlockIndexBuilder(db);
                        break;
                    case CollectionName.Contracts:
                        builder = new ContractIndexBuilder(db);
                        break;
                    case CollectionName.Transactions:
                        builder = new TransactionIndexBuilder(db);
                        break;
                    case CollectionName.TransactionLogs:
                        builder = new TransactionLogIndexBuilder(db);
                        break;
                    default:
                        continue;
                }

                builder.EnsureIndexes();
            }
        }

        public async Task DeleteAllCollections(IMongoDatabase db)
        {
            foreach (CollectionName collectionName in (CollectionName[]) Enum.GetValues(typeof(CollectionName)))
            {
                await db.DropCollectionAsync(collectionName.ToCollectionName()).ConfigureAwait(false);
            }
        }

        public static IIndexerRepositoryFactory Create(IDatabaseSettings settings, bool deleteAllExistingCollections = false)
        {
            string connectionString = settings.ConnectionString;
            string tag = settings.Tag;
            string locale = settings.Locale;

            IndexerRepositoryFactory factoryBase = new IndexerRepositoryFactory(connectionString, tag);
            IMongoDatabase db = factoryBase.CreateDbIfNotExists();

            if (deleteAllExistingCollections)
            {
                factoryBase.DeleteAllCollections(db).Wait();
            }

            factoryBase.CreateCollectionsIfNotExist(db, locale).Wait();

            return factoryBase;
        }
    }
}
