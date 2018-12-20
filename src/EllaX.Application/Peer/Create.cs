using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace EllaX.Application.Peer
{
    public class Create
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
                RuleFor(m => m.RemoteAddress).NotEmpty();
            }
        }

        public class Command : IRequest<Model>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
        }

        public class Model
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public DateTimeOffset FirstSeenDate { get; set; }
            public DateTimeOffset LastSeenDate { get; set; }
        }

        public class Handler : IRequestHandler<Command, Model>
        {
            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                // todo
                return await Task.FromResult(new Model());
            }
        }
    }
}
