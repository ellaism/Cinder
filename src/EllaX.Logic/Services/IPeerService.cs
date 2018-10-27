using System.Threading.Tasks;
using EllaX.Core.Entities;

namespace EllaX.Logic.Services
{
    public interface IPeerService
    {
        Task ProcessPeerAsync(Peer peer);
    }
}
