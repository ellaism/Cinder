using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Paging;

namespace Cinder.UI.Infrastructure.Services
{
    public interface IAddressService
    {
        Task<AddressDto> GetAddressByHash(string hash);
        Task<IPage<TransactionDto>> GetTransactionsAddressByHash(string hash, int? page, int? size);
    }
}
