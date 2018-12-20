using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EllaX.Application.Peer
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Model>> { }

        public class Model
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public DateTimeOffset FirstSeenDate { get; set; }
            public DateTimeOffset LastSeenDate { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                // todo
                return await Task.FromResult(new List<Model>());
            }
        }
    }
}
