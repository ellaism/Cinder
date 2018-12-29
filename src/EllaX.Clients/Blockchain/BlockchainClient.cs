using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;
using EllaX.Extensions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;

namespace EllaX.Clients.Blockchain
{
    public class BlockchainClient : IBlockchainClient
    {
        private readonly IFlurlClient _client;

        public BlockchainClient(IFlurlClientFactory flurlClientFactory, IOptions<BlockchainClientOptions> options)
        {
            _client = flurlClientFactory.Get(options.Value.Endpoint);
        }

        public async Task<Response<ulong>> GetHeightAsync(CancellationToken cancellationToken = default)
        {
            return await _client.Request().PostJsonAsync<ulong>(Message.CreateMessage("eth_blockNumber"), cancellationToken);
        }

        public async Task<Response<BlockResult>> GetBlockAsync(string blockHash, CancellationToken cancellationToken = default)
        {
            return await _client.Request()
                .PostJsonAsync<BlockResult>(Message.CreateMessage("eth_getBlockByHash", new List<object> {blockHash, true}),
                    cancellationToken);
        }

        public async Task<Response<BlockResult>> GetBlockAsync(ulong blockNumber, CancellationToken cancellationToken = default)
        {
            return await _client.Request()
                .PostJsonAsync<BlockResult>(Message.CreateMessage("eth_getBlockByNumber", new List<object> {blockNumber, true}),
                    cancellationToken);
        }

        public async Task<Response<BlockResult>> GetBlockAsync(BlockType type, CancellationToken cancellationToken = default)
        {
            return await _client.Request()
                .PostJsonAsync<BlockResult>(
                    Message.CreateMessage("eth_getBlockByNumber", new List<object> {type.ToString().ToLowerInvariant(), true}),
                    cancellationToken);
        }
    }
}
