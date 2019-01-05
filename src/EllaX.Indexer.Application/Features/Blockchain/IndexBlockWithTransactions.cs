using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Blockchain;
using MediatR;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Block = EllaX.Core.Entities.Block;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class IndexBlockWithTransactions
    {
        public class Command : IRequest
        {
            public uint BlockNumber { get; set; }
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
                BlockWithTransactions response =
                    await _blockchainClient.Web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                        new HexBigInteger(request.BlockNumber));
                var block = _mapper.Map<Block>(response);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
