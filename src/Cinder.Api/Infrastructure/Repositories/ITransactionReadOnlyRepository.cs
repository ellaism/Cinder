using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Documents;

namespace Cinder.Api.Infrastructure.Repositories
{
    public interface ITransactionReadOnlyRepository
    {
        Task<IReadOnlyCollection<CinderTransaction>> GetRecentTransactions(int limit = 10,
            CancellationToken cancellationToken = default);

        Task<CinderTransaction> GetTransactionByHash(string hash, CancellationToken cancellationToken = default);
    }
}
