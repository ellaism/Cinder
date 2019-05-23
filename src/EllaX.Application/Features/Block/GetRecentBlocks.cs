using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EllaX.Application.Features.Block
{
    public class GetRecentBlocks
    {
        public class Query : IRequest<IEnumerable<Model>> { }

        public class Model { }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            public Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
