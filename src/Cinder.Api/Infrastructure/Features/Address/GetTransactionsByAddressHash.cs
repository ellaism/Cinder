using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data.Repositories;
using Cinder.Documents;
using FluentValidation;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Address
{
    public class GetTransactionsByAddressHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.AddressHash).NotEmpty().Length(42);
                RuleFor(m => m.Page).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
                RuleFor(m => m.Size).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
            }
        }

        public class Query : IRequest<IPage<Model>>
        {
            public string AddressHash { get; set; }
            public int? Page { get; set; }
            public int? Size { get; set; }
            public SortOrder Sort { get; set; }
        }

        public class Model
        {
            public string BlockNumber { get; set; }

            public string Hash { get; set; }

            public string Address { get; set; }
        }

        public class Handler : IRequestHandler<Query, IPage<Model>>
        {
            private readonly IAddressTransactionRepository _addressTransactionRepository;

            public Handler(IAddressTransactionRepository addressTransactionRepository)
            {
                _addressTransactionRepository = addressTransactionRepository;
            }

            public async Task<IPage<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IPage<CinderAddressTransaction> transactions =
                    await _addressTransactionRepository.GetTransactionsByAddressHash(request.AddressHash, request.Page,
                        request.Size, request.Sort, cancellationToken);

                return transactions.ToModelPage();
            }
        }
    }

    internal static class Extensions
    {
        public static IPage<GetTransactionsByAddressHash.Model> ToModelPage(this IPage<CinderAddressTransaction> page)
        {
            IEnumerable<GetTransactionsByAddressHash.Model> models = page.Items.Select(block =>
                new GetTransactionsByAddressHash.Model
                {
                    BlockNumber = block.BlockNumber, Hash = block.Hash, Address = block.Address
                });

            return new PagedEnumerable<GetTransactionsByAddressHash.Model>(models, page.Total, page.Page, page.Size);
        }
    }
}
