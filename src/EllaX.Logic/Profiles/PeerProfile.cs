using System;
using AutoMapper;
using EllaX.Core.Dtos;
using EllaX.Core.Models;
using EllaX.Logic.Resolvers;

namespace EllaX.Logic.Profiles
{
    public class PeerProfile : Profile
    {
        public PeerProfile()
        {
            CreateMap<Peer, Peer>().ForMember(dest => dest.FirstSeenDate, opt => opt.Ignore());
            CreateMap<Peer, PeerHealthDto>().ForMember(dest => dest.Age,
                opt => opt.ResolveUsing<DateTimeOffsetToAgeResolver, DateTimeOffset>(src => src.LastSeenDate));
            CreateMap<City, Peer>().ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src =>
                        src.Latitude.HasValue ? Math.Round(src.Latitude.Value, 2) : (decimal?) null))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src =>
                        src.Longitude.HasValue ? Math.Round(src.Longitude.Value, 2) : (decimal?) null))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
