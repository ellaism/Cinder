using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Responses;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class LatestBlock
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
                Response<BigInteger> response = await _blockchainClient.GetLatestBlockAsync(cancellationToken);

                return response.Result;
            }
        }
    }
}
