using System.Threading;
using System.Threading.Tasks;
using EllaX.Core;

namespace EllaX.Logic.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<TDto> GetNetworkHealthAsync<TDto>(bool uniquesOnly = true, int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default);

        Task SnapshotRecentPeerCountAsync(int ageMinutes = Consts.DefaultAgeMinutes);
    }
}
