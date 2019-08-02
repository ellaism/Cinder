using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Documents;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Block
{
    public class GetRecentBlocks
    {
        public class Query : IRequest<IEnumerable<Model>> { }

        public class Model
        {
            public string BlockNumber { get; set; }
            public string Hash { get; set; }
            public ulong Size { get; set; }
            public string Miner { get; set; }
            public ulong Timestamp { get; set; }
            public ulong TransactionCount { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly IBlockReadOnlyRepository _blockRepository;

            public Handler(IBlockReadOnlyRepository blockRepository)
            {
                _blockRepository = blockRepository;
            }

            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<CinderBlock> blocks =
                    await _blockRepository.GetRecentBlocks(cancellationToken: cancellationToken).ConfigureAwait(false);

                return blocks.Select(block => new Model
                {
                    BlockNumber = block.BlockNumber,
                    Hash = block.Hash,
                    Size = ulong.Parse(block.Size),
                    Miner = block.Miner,
                    Timestamp = ulong.Parse(block.Timestamp),
                    TransactionCount = (ulong) block.TransactionCount
                });
            }
        }
    }
}
