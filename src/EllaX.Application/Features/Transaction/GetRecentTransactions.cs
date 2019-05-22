using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EllaX.Application.Features.Transaction
{
    public class GetRecentTransactions
    {
        public class Query : IRequest<IList<Model>> { }

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
