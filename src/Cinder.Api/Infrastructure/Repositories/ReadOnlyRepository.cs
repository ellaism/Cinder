using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public abstract class ReadOnlyRepository<TDocument> : RepositoryBase<TDocument> where TDocument : IDocument
    {
        protected ReadOnlyRepository(IMongoClient client, string databaseName, CollectionName collectionName) : base(client,
            databaseName, collectionName) { }
    }
}
