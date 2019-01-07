using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class GetLatestBlockNumber
    {
        public class Query : IRequest<ulong> { }

        public class Handler : IRequestHandler<Query, ulong>
        {
            private readonly IBlockchainClient _blockchainClient;

            public Handler(IBlockchainClient blockchainClient)
            {
                _blockchainClient = blockchainClient;
            }

            public async Task<ulong> Handle(Query request, CancellationToken cancellationToken)
            {
                ulong response = await _blockchainClient.GetLatestBlockNumberAsync(cancellationToken);

                return response;
            }
        }
    }
}
