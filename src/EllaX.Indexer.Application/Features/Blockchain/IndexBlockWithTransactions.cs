using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients;
using EllaX.Core.Entities;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class IndexBlockWithTransactions
    {
        public class Command : IRequest
        {
            public ulong BlockNumber { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly IBlockchainClient _blockchainClient;
            private readonly IMapper _mapper;

            public Handler(IBlockchainClient blockchainClient, IMapper mapper)
            {
                _blockchainClient = blockchainClient;
                _mapper = mapper;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                Block block = await _blockchainClient.GetBlockWithTransactionsAsync(request.BlockNumber, cancellationToken);
            }
        }
    }
}
