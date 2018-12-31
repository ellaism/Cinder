using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using FluentValidation;
using MediatR;

namespace EllaX.Api.Application.Features.Block
{
    public class Create
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Height).NotEmpty();
            }
        }

        public class Command : IRequest<Model>
        {
            public string Height { get; set; }
        }

        public class Model
        {
            public string Height { get; set; }
        }

        public class Handler : IRequestHandler<Command, Model>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                Core.Entities.Block block = _mapper.Map<Command, Core.Entities.Block>(request);
                await _db.Blocks.AddAsync(block, cancellationToken);
                Model model = _mapper.Map<Core.Entities.Block, Model>(block);
                await _db.SaveChangesAsync(cancellationToken);

                return model;
            }
        }
    }
}
