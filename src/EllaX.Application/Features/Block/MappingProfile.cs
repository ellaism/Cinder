using AutoMapper;

namespace EllaX.Application.Features.Block
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Entities.Block, List.Model>();
            CreateMap<Core.Entities.Block, Detail.Model>();
            CreateMap<Create.Command, Core.Entities.Block>(MemberList.Source);
            CreateMap<Core.Entities.Block, Update.Command>().ReverseMap();
            CreateMap<Core.Entities.Block, Delete.Command>();
        }
    }
}
