using AutoMapper;

namespace EllaX.Api.Application.Features.Block
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Entities.Block, List.Model>();
            CreateMap<Core.Entities.Block, Detail.Model>();
        }
    }
}
