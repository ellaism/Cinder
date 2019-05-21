using AutoMapper;
using EllaX.Application.Features.Peer;
using EllaX.Core.Entities;

namespace EllaX.Application.Profiles
{
    public class PeerProfile : Profile
    {
        public PeerProfile()
        {
            CreateMap<Peer, List.Model>();
            CreateMap<Peer, Detail.Model>();
            CreateMap<Create.Command, Peer>(MemberList.Source);
            CreateMap<Peer, Update.Command>().ReverseMap();
            CreateMap<Peer, Delete.Command>();
        }
    }
}
