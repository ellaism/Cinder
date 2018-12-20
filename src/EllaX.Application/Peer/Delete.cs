using System.Threading;
using System.Threading.Tasks;
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
            protected override Task Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
