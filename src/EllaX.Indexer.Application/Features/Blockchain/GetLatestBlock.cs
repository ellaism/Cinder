using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Blockchain;
using MediatR;
using Nethereum.Hex.HexTypes;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class GetLatestBlock
    {
        public class Query : IRequest<BigInteger> { }

        public class Handler : IRequestHandler<Query, BigInteger>
        {
            private readonly IBlockchainClient _blockchainClient;

            public Handler(IBlockchainClient blockchainClient)
            {
                _blockchainClient = blockchainClient;
            }

            public async Task<BigInteger> Handle(Query request, CancellationToken cancellationToken)
            {
                HexBigInteger response = await _blockchainClient.Web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

                return response.Value;
            }
        }
    }
}
