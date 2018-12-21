using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Features.Block
{
    public class Update
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Height).NotEmpty();
            }
        }

        public class Command : IRequest
        {
            public string Height { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                Core.Entities.Block block = await _db.Blocks.FindAsync(request.Height);
                _mapper.Map(request, block);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
