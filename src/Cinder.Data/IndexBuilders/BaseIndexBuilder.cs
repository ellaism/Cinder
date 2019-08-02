using System;
using System.Linq;
using System.Linq.Expressions;
using Cinder.Documents;
using Cinder.Extensions;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public abstract class BaseIndexBuilder<TDocument> : IIndexBuilder where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> Collection;

        protected BaseIndexBuilder(IMongoDatabase db, CollectionName collectionName)
        {
            Collection = db.GetCollection<TDocument>(collectionName.ToCollectionName());
        }

        public abstract void EnsureIndexes();

        protected void Index(Expression<Func<TDocument, object>> field, bool unique = false)
        {
            Collection.Indexes.CreateOneAsync(new CreateIndexModel<TDocument>(BuildIndexDefinition(field),
                    new CreateIndexOptions {Unique = unique}))
                .Wait();
        }

        protected void Compound(bool unique, params Expression<Func<TDocument, object>>[] fields)
        {
            Collection.Indexes.CreateOneAsync(new CreateIndexModel<TDocument>(
                    Builders<TDocument>.IndexKeys.Combine(fields.Select(BuildIndexDefinition)),
                    new CreateIndexOptions {Unique = unique}))
                .Wait();
        }

        protected IndexKeysDefinition<TDocument> BuildIndexDefinition(Expression<Func<TDocument, object>> field)
        {
            return Builders<TDocument>.IndexKeys.Ascending(field);
        }
    }
}
