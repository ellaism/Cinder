using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Documents;

namespace Cinder.Data.Repositories
{
    public interface IAddressTransactionRepository : Nethereum.BlockchainProcessing.BlockStorage.Repositories.IAddressTransactionRepository
    {
        Task<IPage<CinderAddressTransaction>> GetTransactionsByAddressHash(string addressHash, int? page = null, int? size = null,
            SortOrder sort = SortOrder.Ascending, CancellationToken cancellationToken = default);
    }
}
