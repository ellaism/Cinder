using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions
{
    public static class MongoDbExtensions
    {
        public static async Task<IPage<TDocument>> ToPageAsync<TDocument>(this IFindFluent<TDocument, TDocument> source, int page,
            int size, CancellationToken cancellationToken = default)
        {
            long total = await source.CountDocumentsAsync(cancellationToken);
            List<TDocument> documents = await source.Skip((page - 1) * size).Limit(size).ToListAsync(cancellationToken);

            return new PagedEnumerable<TDocument>(documents, (int) total, page, size);
        }
    }
}
