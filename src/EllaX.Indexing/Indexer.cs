using System.Threading.Tasks;

namespace EllaX.Indexing
{
    public abstract class Indexer : IIndexer
    {
        public abstract Task Run();
    }
}
