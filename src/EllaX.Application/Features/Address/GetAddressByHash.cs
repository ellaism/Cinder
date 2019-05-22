using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Features.Address
{
    public class GetAddressByHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Hash).NotEmpty();
            }
        }

        public class Query : IRequest<IList<Model>>
        {
            public string Hash { get; set; }
        }

        public class Model { }

        public class Handler : IRequestHandler<Query, IList<Model>>
        {
            public Task<IList<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
