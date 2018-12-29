using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Indexer.Indexing
{
    public interface IRunnable
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
