using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface IAddressService
    {
        Task<AddressDto> GetAddressByHash(string hash);
    }
}
