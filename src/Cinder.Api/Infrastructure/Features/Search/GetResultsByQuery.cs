using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Search
{
    public class GetResultsByQuery
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Q).NotEmpty();
            }
        }

        public class Query : IRequest<Model>
        {
            public string Q { get; set; }
        }

        public class Model
        {
            public string Type { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            Task<Model> IRequestHandler<Query, Model>.Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
