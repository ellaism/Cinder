using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Parity.NetPeers;
using EllaX.Extensions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;

namespace EllaX.Clients.Network
{
    public class NetworkClient : INetworkClient
    {
        private readonly IFlurlClient _client;

        public NetworkClient(IFlurlClientFactory flurlClientFactory, IOptions<NetworkClientOptions> options)
        {
            _client = flurlClientFactory.Get(options.Value.Endpoint);
        }

        public async Task<Response<NetPeerResult>> GetNetPeersAsync(string host, CancellationToken cancellationToken = default)
        {
            return await _client.Request().PostJsonAsync<NetPeerResult>(Message.Create("parity_netPeers"), cancellationToken);
        }
    }
}
