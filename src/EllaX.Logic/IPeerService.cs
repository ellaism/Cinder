using System.Threading.Tasks;
using EllaX.Core.Entities;

namespace EllaX.Logic
{
    public interface IPeerService
    {
        Task ProcessPeerAsync(Peer peer);
    }
}
