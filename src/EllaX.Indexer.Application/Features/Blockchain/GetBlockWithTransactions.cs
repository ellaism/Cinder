using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Blockchain;
using MediatR;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

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
            public BigInteger Number { get; set; }
            public string BlockHash { get; set; }
            public string Author { get; set; }
            public string[] SealFields { get; set; }
            public string ParentHash { get; set; }
            public string Nonce { get; set; }
            public string Sha3Uncles { get; set; }
            public string LogsBloom { get; set; }
            public string TransactionsRoot { get; set; }
            public string StateRoot { get; set; }
            public string ReceiptsRoot { get; set; }
            public string Miner { get; set; }
            public BigInteger Difficulty { get; set; }
            public BigInteger TotalDifficulty { get; set; }
            public string ExtraData { get; set; }
            public BigInteger Size { get; set; }
            public BigInteger GasLimit { get; set; }
            public BigInteger GasUsed { get; set; }
            public BigInteger Timestamp { get; set; }
            public string[] Uncles { get; set; }
            public Transaction[] Transactions { get; set; }

            public class Transaction
            {
                public string TransactionHash { get; set; }
                public BigInteger TransactionIndex { get; set; }
                public string BlockHash { get; set; }
                public BigInteger BlockNumber { get; set; }
                public string From { get; set; }
                public string To { get; set; }
                public BigInteger Gas { get; set; }
                public BigInteger GasPrice { get; set; }
                public BigInteger Value { get; set; }
                public string Input { get; set; }
                public BigInteger Nonce { get; set; }
            }
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
                BlockWithTransactions response =
                    await _blockchainClient.Web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                        new HexBigInteger(request.BlockNumber));
                Model result = _mapper.Map<Model>(response);

                return result;
            }
        }
    }
}
