using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Core.Models;

namespace EllaX.Logic
{
    public interface IStatisticsService
    {
        void AddPeer(Peer peer);
        Task<IReadOnlyList<Health>> GetHealthAsync();
    }
}
