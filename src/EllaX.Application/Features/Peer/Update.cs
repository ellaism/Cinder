using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Features.Peer
{
    public class Update
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
                RuleFor(m => m.RemoteAddress).NotEmpty();
            }
        }

        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
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
                Core.Entities.Peer peer = await _db.Peers.FindAsync(request.Id);
                _mapper.Map(request, peer);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
