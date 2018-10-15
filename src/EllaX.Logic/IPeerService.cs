using System.Threading.Tasks;
using EllaX.Core.Models;

namespace EllaX.Logic
{
    public interface IPeerService
    {
        Task ProcessPeerAsync(Peer peer);
    }
}
