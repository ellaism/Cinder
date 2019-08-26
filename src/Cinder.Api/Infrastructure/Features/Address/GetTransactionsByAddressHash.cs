using System.Threading;
using System.Threading.Tasks;
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

        public class Query : IRequest<Model>
        {
            public string AddressHash { get; set; }
            public int? Page { get; set; }
            public int? Size { get; set; }
        }

        public class Model
        {
            public string Hash { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                // TODO
                return Task.FromResult(new Model {Hash = request.AddressHash});
            }
        }
    }
}
