using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;
using Microsoft.Extensions.Options;

namespace EllaX.Clients.Blockchain
{
    public class BlockchainClient : Client, IBlockchainClient
    {
        private readonly IOptions<BlockchainClientOptions> _options;

        public BlockchainClient(HttpClient client, IOptions<BlockchainClientOptions> options) : base(client)
        {
            _options = options;
        }

        public async Task<Response<ulong>> GetHeight(CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage request =
                CreateRequest(Message.CreateMessage("eth_blockNumber"), _options.Value.Endpoint))
            {
                return await SendAsync<ulong>(request, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Response<BlockResult>> GetBlock(string blockHash,
            CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage request =
                CreateRequest(Message.CreateMessage("eth_getBlockByHash", new List<object> {blockHash, true}),
                    _options.Value.Endpoint))
            {
                return await SendAsync<BlockResult>(request, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Response<BlockResult>> GetBlock(ulong blockNumber,
            CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage request =
                CreateRequest(Message.CreateMessage("eth_getBlockByNumber", new List<object> {blockNumber, true}),
                    _options.Value.Endpoint))
            {
                return await SendAsync<BlockResult>(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
