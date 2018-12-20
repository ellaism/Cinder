using System.Threading;
using System.Threading.Tasks;
using EllaX.Application.Services.Results.Location;

namespace EllaX.Application.Services
{
    public interface ILocationService
    {
        Task<CityResult> GetCityByIpAsync(string ip, CancellationToken cancellationToken = default);
    }
}
