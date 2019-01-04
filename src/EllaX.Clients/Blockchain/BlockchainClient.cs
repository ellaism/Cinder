using Microsoft.Extensions.Options;
using Nethereum.Web3;

namespace EllaX.Clients.Blockchain
{
    public class BlockchainClient : IBlockchainClient
    {
        public BlockchainClient(IOptions<BlockchainClientOptions> options)
        {
            Web3 = new Web3(options.Value.Endpoint);
        }

        public Web3 Web3 { get; }
    }
}
