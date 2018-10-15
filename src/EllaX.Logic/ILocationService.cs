using EllaX.Core.Models;

namespace EllaX.Logic
{
    public interface ILocationService
    {
        City GetCityByIp(string ip);
    }
}
