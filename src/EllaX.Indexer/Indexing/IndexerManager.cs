using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Indexer.Indexing
{
    public class IndexerManager : IIndexerManager
    {
        private readonly IEnumerable<IIndexer> _indexers;

        public IndexerManager(IEnumerable<IIndexer> indexers)
        {
            _indexers = indexers;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Task[] tasks = _indexers.Select(indexer => indexer.RunAsync(cancellationToken)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
