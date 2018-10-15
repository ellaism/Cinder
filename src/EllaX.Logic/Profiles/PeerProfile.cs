using AutoMapper;
using EllaX.Core.Models;

namespace EllaX.Logic.Profiles
{
    public class PeerProfile : Profile
    {
        public PeerProfile()
        {
            CreateMap<Peer, Peer>().ForMember(dest => dest.FirstSeenDate, opt => opt.Ignore());
            CreateMap<Peer, Health>().ForMember(dest => dest.Location,
                opt => opt.MapFrom(src => $"{src.City} {src.Country}".Trim()));
            CreateMap<City, Peer>().ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
