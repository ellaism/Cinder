using System.Threading;
using System.Threading.Tasks;
using EllaX.Data;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Peer
{
    public class Delete
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
            }
        }

        public class Command : IRequest
        {
            public string Id { get; set; }
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
                Core.Entities.Peer peer = await _db.Peers.FindAsync(request.Id);
                _db.Peers.Remove(peer);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
