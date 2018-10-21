using AutoMapper;
using EllaX.Logic.Models;
using MaxMind.GeoIP2.Responses;

namespace EllaX.Logic.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<CityResponse, City>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.IsoCode))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude));
        }
    }
}
