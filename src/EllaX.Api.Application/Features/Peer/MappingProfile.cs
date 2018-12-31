using AutoMapper;

namespace EllaX.Api.Application.Features.Peer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Entities.Peer, List.Model>();
            CreateMap<Core.Entities.Peer, Detail.Model>();
            CreateMap<Create.Command, Core.Entities.Peer>(MemberList.Source);
            CreateMap<Core.Entities.Peer, Update.Command>().ReverseMap();
            CreateMap<Core.Entities.Peer, Delete.Command>();
        }
    }
}
