using System.Threading.Tasks;
using EllaX.Logic.Models;

namespace EllaX.Logic
{
    public interface ILocationService
    {
        Task<City> GetCityByIpAsync(string ip);
    }
}
