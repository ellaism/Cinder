using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Services.Results.Location;

namespace EllaX.Logic.Services
{
    public interface ILocationService
    {
        Task<CityResult> GetCityByIpAsync(string ip, CancellationToken cancellationToken = default);
    }
}
