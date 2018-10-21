using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Clients.Options;
using EllaX.Logic.Clients.Requests;
using EllaX.Logic.Clients.Responses;
using EllaX.Logic.Clients.Responses.Parity.NetPeers;
using Microsoft.Extensions.Options;

namespace EllaX.Logic.Clients
{
    public class NetworkClient : Client, INetworkClient
    {
        private readonly IOptions<NetworkClientOptions> _options;

        public NetworkClient(HttpClient client, IOptions<NetworkClientOptions> options) : base(client)
        {
            _options = options;
        }

        public IReadOnlyCollection<string> Nodes => _options.Value?.Nodes.ToArray() ?? new string[0];

        public async Task<Response<NetPeerResult>> GetNetPeersAsync(string host,
            CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage request = CreateRequest(Message.CreateMessage("parity_netPeers"), host))
            {
                return await SendAsync<NetPeerResult>(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
