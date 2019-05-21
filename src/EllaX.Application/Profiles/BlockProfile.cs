using AutoMapper;
using EllaX.Application.Features.Block;
using EllaX.Core.Entities;

namespace EllaX.Application.Profiles
{
    public class BlockProfile : Profile
    {
        public BlockProfile()
        {
            CreateMap<Block, List.Model>();
            CreateMap<Block, Detail.Model>();
        }
    }
}
