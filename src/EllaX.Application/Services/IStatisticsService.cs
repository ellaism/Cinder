using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EllaX.Application.Services
{
    public interface IStatisticsService
    {
        Task CreateRecentPeerSnapshotAsync(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default);

        Task<TDto> GetNetworkHealthAsync<TDto>(bool uniquesOnly = true, int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default);

        Task ProcessPeersAsync(IEnumerable<Peer> peers, CancellationToken cancellationToken = default);
    }
}
