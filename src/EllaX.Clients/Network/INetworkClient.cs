using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Parity.NetPeers;

namespace EllaX.Clients.Network
{
    public interface INetworkClient
    {
        Task<Response<NetPeerResult>> GetNetPeersAsync(string host, CancellationToken cancellationToken = default);
    }
}
