using System.Threading;
using System.Threading.Tasks;

namespace Cinder.Indexer.Infrastructure
{
    public interface IAddressRefresherRunner
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
