using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class LatestBlock
    {
        public class Query : IRequest<Model> { }

        public class Model
        {
            public string Height { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IBlockchainClient _blockchainClient;

            public Handler(IBlockchainClient blockchainClient)
            {
                _blockchainClient = blockchainClient;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                Response<BlockResult> response = await _blockchainClient.GetBlockAsync(BlockType.Latest, cancellationToken);

                return null;
            }
        }
    }
}
