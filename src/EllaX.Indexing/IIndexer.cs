using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Indexing
{
    public interface IIndexer
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
