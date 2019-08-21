using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Api.Infrastructure.Services;
using Cinder.Documents;
using FluentValidation;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Block
{
    public class GetRecentBlocks
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Limit).GreaterThanOrEqualTo(1);
            }
        }

        public class Query : IRequest<IEnumerable<Model>>
        {
            public int? Limit { get; set; }
        }

        public class Model
        {
            public string BlockNumber { get; set; }
            public string Hash { get; set; }
            public ulong Size { get; set; }
            public string Miner { get; set; }
            public string MinerDisplay { get; set; }
            public ulong Timestamp { get; set; }
            public ulong TransactionCount { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly IBlockReadOnlyRepository _blockRepository;
            private readonly IMinerLookupService _minerLookupService;

            public Handler(IBlockReadOnlyRepository blockRepository, IMinerLookupService minerLookupService)
            {
                _blockRepository = blockRepository;
                _minerLookupService = minerLookupService;
            }

            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<CinderBlock> blocks =
                    await _blockRepository.GetRecentBlocks(request.Limit, cancellationToken).ConfigureAwait(false);

                return blocks.Select(block => new Model
                {
                    BlockNumber = block.BlockNumber,
                    Hash = block.Hash,
                    Size = ulong.Parse(block.Size),
                    Miner = block.Miner,
                    MinerDisplay = _minerLookupService.GetByAddressOrDefault(block.Miner),
                    Timestamp = ulong.Parse(block.Timestamp),
                    TransactionCount = (ulong) block.TransactionCount
                });
            }
        }
    }
}
