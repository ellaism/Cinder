using System.Threading.Tasks;

namespace EllaX.Logic.Indexing
{
    public abstract class Indexer : IIndexer
    {
        public abstract Task Run();
    }
}
