using System.Threading;
using System.Threading.Tasks;
using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Indexer.Infrastructure.Repositories
{
    public abstract class IndexerRepositoryBase<TDocument> : RepositoryBase<TDocument> where TDocument : IDocument
    {
        protected IndexerRepositoryBase(IMongoClient client, string databaseName, CollectionName collectionName) : base(client,
            databaseName, collectionName) { }

        protected async Task UpsertDocumentAsync(TDocument updatedDocument, CancellationToken cancellationToken = default)
        {
            await Collection.ReplaceOneAsync(CreateDocumentFilter(updatedDocument), updatedDocument,
                new UpdateOptions {IsUpsert = true}, cancellationToken);
        }

        //protected async Task UpsertDocumentsAsync(IEnumerable<TDocument> updatedDocuments,
        //    CancellationToken cancellationToken = default)
        //{
        //    IList<TDocument> enumerable = updatedDocuments as TDocument[] ?? updatedDocuments.ToArray();
        //    WriteModel<TDocument>[] models = new WriteModel<TDocument>[enumerable.Count];

        //    for (int i = 0; i < enumerable.Count; i++)
        //    {
        //        models[i] = new ReplaceOneModel<TDocument>(CreateDocumentFilter(enumerable[i]), enumerable[i]) {IsUpsert = true};
        //    }

        //    await Collection.BulkWriteAsync(models, cancellationToken: cancellationToken);
        //}
    }
}
