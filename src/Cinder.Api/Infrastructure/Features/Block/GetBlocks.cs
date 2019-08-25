using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Api.Infrastructure.Services;
using Cinder.Core.Paging;
using Cinder.Documents;
using FluentValidation;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Block
{
    public class GetBlocks
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Page).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
                RuleFor(m => m.Size).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
            }
        }

        public class Query : IRequest<IPage<Model>>
        {
            public int? Page { get; set; }
            public int? Size { get; set; }
            public SortOrder Sort { get; set; }
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
            public string MinerDisplay { get; set; }
            public ulong GasLimit { get; set; }
            public ulong GasUsed { get; set; }
            public ulong Timestamp { get; set; }
            public ulong TransactionCount { get; set; }
            public string[] Uncles { get; set; }
            public ulong UncleCount { get; set; }
            public string Sha3Uncles { get; set; }
        }

        public class Handler : IRequestHandler<Query, IPage<Model>>
        {
            private readonly IBlockReadOnlyRepository _blockRepository;
            private readonly IMinerLookupService _minerLookupService;

            public Handler(IBlockReadOnlyRepository blockRepository, IMinerLookupService minerLookupService)
            {
                _blockRepository = blockRepository;
                _minerLookupService = minerLookupService;
            }

            public async Task<IPage<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IPage<CinderBlock> blocks = await _blockRepository.GetBlocks(request.Page, request.Size, request.Sort, cancellationToken)
                    .ConfigureAwait(false);

                return blocks.ToModelPage(_minerLookupService);
            }
        }
    }

    internal static class Extensions
    {
        public static IPage<GetBlocks.Model> ToModelPage(this IPage<CinderBlock> page, IMinerLookupService minerLookupService)
        {
            IEnumerable<GetBlocks.Model> models = page.Items.Select(block => new GetBlocks.Model
            {
                BlockNumber = block.BlockNumber,
                Difficulty = block.Difficulty,
                ExtraData = block.ExtraData,
                GasLimit = ulong.Parse(block.GasLimit),
                GasUsed = ulong.Parse(block.GasUsed),
                Hash = block.Hash,
                Miner = block.Miner,
                MinerDisplay = minerLookupService.GetByAddressOrDefault(block.Miner),
                Nonce = block.Nonce,
                ParentHash = block.ParentHash,
                Size = ulong.Parse(block.Size),
                Timestamp = ulong.Parse(block.Timestamp),
                TransactionCount = (ulong) block.TransactionCount,
                TotalDifficulty = block.TotalDifficulty,
                Uncles = block.Uncles,
                UncleCount = (ulong) block.UncleCount,
                Sha3Uncles = block.Sha3Uncles
            });

            return new PagedEnumerable<GetBlocks.Model>(models, page.Total, page.Page, page.Size);
        }
    }
}
