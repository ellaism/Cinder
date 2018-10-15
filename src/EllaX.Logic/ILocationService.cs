using System.Threading.Tasks;
using EllaX.Core.Models;

namespace EllaX.Logic
{
    public interface ILocationService
    {
        Task<City> GetCityByIpAsync(string ip);
    }
}
