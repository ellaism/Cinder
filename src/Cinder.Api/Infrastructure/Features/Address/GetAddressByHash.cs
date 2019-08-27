using System;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Data.Repositories;
using Cinder.Documents;
using FluentValidation;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Address
{
    public class GetAddressByHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Hash).NotEmpty().Length(42);
            }
        }

        public class Query : IRequest<Model>
        {
            public string Hash { get; set; }
        }

        public class Model
        {
            public string Hash { get; set; }
            public string Balance { get; set; }
            public ulong BlocksMined { get; set; }
            public ulong TransactionCount { get; set; }
            public DateTimeOffset CacheDate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IAddressRepository _addressRepository;

            public Handler(IAddressRepository addressRepository)
            {
                _addressRepository = addressRepository;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                CinderAddress address = await _addressRepository.GetAddressByHash(request.Hash, cancellationToken);

                if (address == null)
                {
                    return null;
                }

                return new Model
                {
                    Hash = address.Hash,
                    Balance = address.Balance,
                    BlocksMined = address.BlocksMined,
                    TransactionCount = address.TransactionCount,
                    CacheDate = address.CacheDate
                };
            }
        }
    }
}
