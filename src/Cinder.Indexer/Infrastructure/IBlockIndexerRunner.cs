using System.Threading;
using System.Threading.Tasks;

namespace Cinder.Indexer.Infrastructure
{
    public interface IBlockIndexerRunner
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
