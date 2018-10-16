using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Logic
{
    public interface IBlockchainService
    {
        Task GetHealthAsync(IList<string> hosts, CancellationToken cancellationToken = default);
    }
}
