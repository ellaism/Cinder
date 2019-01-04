using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class GetBlockWithTransactions
    {
        public class Query : IRequest<Model>
        {
            public uint BlockNumber { get; set; }
        }

        public class Model
        {
            //public BigInteger GasUsed { get; set; }
            //public BigInteger GasLimit { get; set; }
            //public BigInteger Size { get; set; }
            //public BigInteger TotalDifficulty { get; set; }
            //public BigInteger Difficulty { get; set; }
            //public BigInteger Timestamp { get; set; }
            //public BigInteger Number { get; set; }
            public string ExtraData { get; set; }
            public string Miner { get; set; }
            public string ReceiptsRoot { get; set; }
            public string TransactionsRoot { get; set; }
            public string LogsBloom { get; set; }
            public string Sha3Uncles { get; set; }
            public string Nonce { get; set; }
            public string ParentHash { get; set; }
            public string BlockHash { get; set; }
            public string StateRoot { get; set; }
            public int TransactionsCount { get; set; }
            public bool IsIndexed { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IBlockchainClient _blockchainClient;
            private readonly IMapper _mapper;

            public Handler(IBlockchainClient blockchainClient, IMapper mapper)
            {
                _blockchainClient = blockchainClient;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                Response<BlockResult> response =
                    await _blockchainClient.GetBlockWithTransactionsAsync(request.BlockNumber, cancellationToken);
                Model result = _mapper.Map<Model>(response.Result);

                return result;
            }
        }
    }
}
