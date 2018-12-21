using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using EllaX.Extensions;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Features.Block
{
    public class List
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Page).GreaterThanOrEqualTo(1);
            }
        }

        public class Query : IRequest<IEnumerable<Model>>
        {
            public int Page { get; set; } = 1;
        }

        public class Model
        {
            public string Height { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Model> models = await _db.Blocks.ProjectToListAsync<Model>(_mapper.ConfigurationProvider);

                return models;
            }
        }
    }
}
