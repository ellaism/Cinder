using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using FluentValidation;
using MediatR;
using static EllaX.Core.Constants;

namespace EllaX.Application.Features.Peer
{
    public class Update
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Id).NotEmpty().Length(Entities.Peer.IdMaxLength);
                RuleFor(command => command.Name).MaximumLength(Entities.Peer.NameMaxLength);
                RuleFor(command => command.LocalAddress).NotEmpty().MaximumLength(Entities.Peer.AddressMaxLength);
                RuleFor(command => command.RemoteAddress).NotEmpty().MaximumLength(Entities.Peer.AddressMaxLength);
                RuleFor(command => command.City).MaximumLength(Entities.Peer.CityMaxLength);
                RuleFor(command => command.Country).MaximumLength(Entities.Peer.CountryMaxLength);
            }
        }

        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
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
