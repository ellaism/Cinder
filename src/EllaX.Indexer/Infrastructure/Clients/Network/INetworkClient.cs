using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Indexer.Infrastructure.Clients.Responses;
using EllaX.Indexer.Infrastructure.Clients.Responses.Parity.NetPeers;

namespace EllaX.Indexer.Infrastructure.Clients.Network
{
    public interface INetworkClient
    {
        IReadOnlyCollection<string> Nodes { get; }
        Task<Response<NetPeerResult>> GetNetPeersAsync(string host, CancellationToken cancellationToken = default);
    }
}
