using Cinder.Documents;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;

namespace Cinder.Data
{
    public abstract class RepositoryFactoryBase : IRepositoryFactory
    {
        protected readonly IMongoClient Client;
        protected readonly string DatabaseName;

        protected RepositoryFactoryBase(string connectionString, string dbTag)
        {
            DatabaseName = "cinder" + dbTag;
            Client = new MongoClient(connectionString);

            CreateMaps();
        }

        public abstract TRepository CreateRepository<TRepository>() where TRepository : IRepository;

        protected void CreateMaps()
        {
            BsonClassMap.RegisterClassMap<TableRow>(map =>
            {
                map.AutoMap();
                map.UnmapMember(m => m.RowIndex);
                map.UnmapMember(m => m.RowCreated);
                map.UnmapMember(m => m.RowUpdated);
                map.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<CinderAddress>();
            BsonClassMap.RegisterClassMap<CinderAddressTransaction>();
            BsonClassMap.RegisterClassMap<CinderBlock>();
            BsonClassMap.RegisterClassMap<CinderContract>();
            BsonClassMap.RegisterClassMap<CinderTransaction>();
            BsonClassMap.RegisterClassMap<CinderTransactionLog>();

            BsonClassMap.RegisterClassMap<BlockProgress>(map =>
            {
                map.AutoMap();
                map.UnmapMember(m => m.LastBlockProcessed);
                map.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<CinderBlockProgress>();
        }
    }
}
