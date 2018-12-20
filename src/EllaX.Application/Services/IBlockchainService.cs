using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Application.Services
{
    public interface IBlockchainService
    {
        Task<IReadOnlyCollection<PeerItem>> GetPeersAsync(CancellationToken cancellationToken = default);
    }
}
