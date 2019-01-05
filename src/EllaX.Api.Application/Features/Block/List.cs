using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using EllaX.Data.Pagination;
using EllaX.Extensions;
using FluentValidation;
using MediatR;

namespace EllaX.Api.Application.Features.Block
{
    public class List
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Page).GreaterThanOrEqualTo(1);
                RuleFor(m => m.Size).InclusiveBetween(1, 100);
            }
        }

        public class Query : IRequest<IPaginatedResult<Model>>
        {
            public int Page { get; set; } = 1;
            public int Size { get; set; } = 10;
        }

        public class Model
        {
            public string Number { get; set; }
        }

        public class Handler : IRequestHandler<Query, IPaginatedResult<Model>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<IPaginatedResult<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IPaginatedResult<Model> models =
                    await _db.Blocks.ProjectToPaginatedResultAsync<Model, Core.Entities.Block>(_mapper, request.Page,
                        request.Size);

                return models;
            }
        }
    }
}
