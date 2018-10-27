using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Logic.Services
{
    public interface IBlockchainService
    {
        Task GetHealthAsync(CancellationToken cancellationToken = default);
    }
}
