using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses.Parity.NetPeers;

namespace EllaX.Logic.Services
{
    public interface IBlockchainService
    {
        Task<IReadOnlyCollection<PeerItem>> GetPeersAsync(CancellationToken cancellationToken = default);
    }
}
