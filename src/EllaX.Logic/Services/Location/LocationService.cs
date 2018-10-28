using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Logic.Services.Location.Results;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Options;

namespace EllaX.Logic.Services.Location
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

        public Task<CityResult> GetCityByIpAsync(string ip, CancellationToken cancellationToken = default)
        {
            CityResponse response = _reader.City(ip);

            return Task.FromResult(_mapper.Map<CityResult>(response));
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
