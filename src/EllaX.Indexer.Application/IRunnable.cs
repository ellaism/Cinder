using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Indexer.Application
{
    public interface IRunnable
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
