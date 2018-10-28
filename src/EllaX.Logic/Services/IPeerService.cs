using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Entities;

namespace EllaX.Logic.Services
{
    public interface IPeerService
    {
        Task ProcessPeersAsync(IEnumerable<Peer> peers, CancellationToken cancellationToken = default);
    }
}
