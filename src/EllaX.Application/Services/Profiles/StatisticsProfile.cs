using System;
using EllaX.Application.Resolvers;
using EllaX.Application.Services.Results.Location;
using EllaX.Application.Services.Results.Statistics;

namespace EllaX.Application.Services.Profiles
{
    public class StatisticsProfile : Profile
    {
        public StatisticsProfile()
        {
            CreateMap<Peer, Peer>().ForMember(dest => dest.FirstSeenDate, opt => opt.Ignore());
            CreateMap<Peer, PeerHealthDto>().ForMember(dest => dest.Age,
                opt => opt.MapFrom<DateTimeOffsetToAgeResolver, DateTimeOffset>(src => src.LastSeenDate));
            CreateMap<CityResult, Peer>().ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src =>
                        src.Latitude.HasValue ? Math.Round(src.Latitude.Value, 2) : (decimal?) null))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src =>
                        src.Longitude.HasValue ? Math.Round(src.Longitude.Value, 2) : (decimal?) null))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<NetworkHealthResult, NetworkHealthResultDto>();
        }
    }
}
