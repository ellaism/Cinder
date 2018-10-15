using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Models;

namespace EllaX.Logic
{
    public interface IStatisticsService
    {
        Task<IReadOnlyList<Health>> GetHealthAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
