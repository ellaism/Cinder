using System;
using AutoMapper;
using EllaX.Core.Models;
using EllaX.Logic.Options;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Options;

namespace EllaX.Logic
{
    public class LocationService : ILocationService, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly LocationOptions _options;
        private readonly DatabaseReader _reader;

        public LocationService(IOptions<LocationOptions> options, IMapper mapper)
        {
            _mapper = mapper;
            _options = options.Value;
            _reader = new DatabaseReader(_options.ConnectionString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public City GetCityByIp(string ip)
        {
            CityResponse response = _reader.City(ip);

            return _mapper.Map<City>(response);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reader?.Dispose();
            }
        }
    }
}
