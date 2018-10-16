using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Clients.Responses;
using EllaX.Logic.Clients.Responses.Parity.NetPeers;

namespace EllaX.Logic.Clients
{
    public interface IBlockchainClient
    {
        Task<Response<NetPeerResult>> GetNetPeersAsync(string host, CancellationToken cancellationToken = default);
    }
}
