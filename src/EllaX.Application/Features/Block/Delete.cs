using System.Threading;
using System.Threading.Tasks;
using EllaX.Data;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Features.Block
{
    public class Delete
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

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                Core.Entities.Block block = await _db.Blocks.FindAsync(request.Height);
                _db.Blocks.Remove(block);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
