using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Logic.Indexing
{
    public interface IRunnable
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
