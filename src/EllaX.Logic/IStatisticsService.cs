using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core;

namespace EllaX.Logic
{
    public interface IStatisticsService
    {
        Task<IReadOnlyCollection<TDto>> GetHealthAsync<TDto>(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default);

        Task SnapshotRecentPeerCountAsync(int ageMinutes = Consts.DefaultAgeMinutes);
    }
}
