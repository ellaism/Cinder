using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core;

namespace EllaX.Logic
{
    public interface IStatisticsService
    {
        Task<IReadOnlyList<TDto>> GetHealthAsync<TDto>(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default);
    }
}
