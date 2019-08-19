using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Documents;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Block
{
    public class GetBlockByNumber
    {
        public class Query : IRequest<Model>
        {
            public ulong Number { get; set; }
        }

        public class Model
        {
            public string BlockNumber { get; set; }
            public string Hash { get; set; }
            public string ParentHash { get; set; }
            public string Nonce { get; set; }
            public string ExtraData { get; set; }
            public string Difficulty { get; set; }
            public string TotalDifficulty { get; set; }
            public ulong Size { get; set; }
            public string Miner { get; set; }
            public ulong GasLimit { get; set; }
            public ulong GasUsed { get; set; }
            public ulong Timestamp { get; set; }
            public ulong TransactionCount { get; set; }
            public string[] Uncles { get; set; }
            public ulong UncleCount { get; set; }
            public string Sha3Uncles { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IBlockReadOnlyRepository _blockRepository;

            public Handler(IBlockReadOnlyRepository blockRepository)
            {
                _blockRepository = blockRepository;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                CinderBlock block = await _blockRepository.GetBlockByNumber(request.Number, cancellationToken)
                    .ConfigureAwait(false);

                return new Model
                {
                    BlockNumber = block.BlockNumber,
                    Difficulty = block.Difficulty,
                    ExtraData = block.ExtraData,
                    GasLimit = ulong.Parse(block.GasLimit),
                    GasUsed = ulong.Parse(block.GasUsed),
                    Hash = block.Hash,
                    Miner = block.Miner,
                    Nonce = block.Nonce,
                    ParentHash = block.ParentHash,
                    Size = ulong.Parse(block.Size),
                    Timestamp = ulong.Parse(block.Timestamp),
                    TransactionCount = (ulong) block.TransactionCount,
                    TotalDifficulty = block.TotalDifficulty,
                    Uncles = block.Uncles,
                    UncleCount = (ulong) block.UncleCount,
                    Sha3Uncles = block.Sha3Uncles
                };
            }
        }
    }
}
