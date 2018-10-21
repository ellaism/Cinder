using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Logic
{
    public interface IBlockchainService
    {
        Task GetHealthAsync(CancellationToken cancellationToken = default);
    }
}
