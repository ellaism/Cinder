using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Extensions;
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

        public async Task<Response<BigInteger>> GetLatestBlockAsync(CancellationToken cancellationToken = default)
        {
            return await _client.Request().PostJsonAsync<BigInteger>(Message.Create("eth_blockNumber"), cancellationToken);
        }

        public async Task<Response<BlockResult>> GetBlockWithTransactionsAsync(BigInteger blockNumber,
            CancellationToken cancellationToken = default)
        {
            return await _client.Request()
                .PostJsonAsync<BlockResult>(Message.Create("eth_getBlockByNumber", new List<object> {blockNumber.ToHex(), true}),
                    cancellationToken);
        }
    }
}
