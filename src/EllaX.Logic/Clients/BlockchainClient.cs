using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Clients.Requests;
using EllaX.Logic.Clients.Responses;
using EllaX.Logic.Clients.Responses.Parity.NetPeers;

namespace EllaX.Logic.Clients
{
    public class BlockchainClient : Client, IBlockchainClient
    {
        public BlockchainClient(HttpClient client) : base(client) { }

        public async Task<Response<NetPeerResult>> GetNetPeersAsync(string host,
            CancellationToken ctx = default(CancellationToken))
        {
            using (HttpRequestMessage request = CreateRequest(Message.CreateMessage("parity_netPeers"), host))
            {
                return await SendAsync<NetPeerResult>(request, ctx).ConfigureAwait(false);
            }
        }
    }
}
