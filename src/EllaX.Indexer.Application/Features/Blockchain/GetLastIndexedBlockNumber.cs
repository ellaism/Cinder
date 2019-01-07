using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class GetLastIndexedBlockNumber
    {
        public class Query : IRequest<ulong> { }

        public class Handler : IRequestHandler<Query, ulong>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<ulong> Handle(Query request, CancellationToken cancellationToken)
            {
                ulong response = await _db.Blocks.OrderByDescending(block => block.BlockNumber).Select(block => block.BlockNumber).SingleOrDefaultAsync();

                return response;
            }
        }
    }
}
