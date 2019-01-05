using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class GetLastIndexedBlock
    {
        public class Query : IRequest<Model> { }

        public class Model
        {
            public string Number { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            //private readonly ApplicationDbContext _db;
            //private readonly IMapper _mapper;

            //public Handler(ApplicationDbContext db, IMapper mapper)
            //{
            //    _db = db;
            //    _mapper = mapper;
            //}

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                //Model model =
                //    await QueryableExtensions.ProjectToSingleOrDefaultAsync<Model>(
                //        _db.Blocks.OrderByDescending(block => block.Height), _mapper.ConfigurationProvider);

                //return model;

                return new Model();
            }
        }
    }
}
