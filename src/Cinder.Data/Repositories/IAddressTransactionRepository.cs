using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cinder.Data.Repositories
{
    public interface IAddressTransactionRepository : Nethereum.BlockchainProcessing.BlockStorage.Repositories.IAddressTransactionRepository
    {
        Task<IEnumerable<string>> GetUniqueAddresses(CancellationToken cancellationToken = default);
    }
}
