using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic.Services.Location.Results;

namespace EllaX.Logic.Services.Location
{
    public interface ILocationService
    {
        Task<CityResult> GetCityByIpAsync(string ip, CancellationToken cancellationToken = default);
    }
}
