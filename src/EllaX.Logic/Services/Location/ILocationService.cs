using System.Threading.Tasks;
using EllaX.Logic.Services.Location.Models;

namespace EllaX.Logic.Services.Location
{
    public interface ILocationService
    {
        Task<City> GetCityByIpAsync(string ip);
    }
}
