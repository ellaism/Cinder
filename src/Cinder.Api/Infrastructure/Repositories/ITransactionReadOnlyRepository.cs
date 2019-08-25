using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Documents;

namespace Cinder.Api.Infrastructure.Repositories
{
    public interface ITransactionReadOnlyRepository
    {
        Task<IPage<CinderTransaction>> GetTransactions(int? page = null, int? size = null,
            CancellationToken cancellationToken = default);

        Task<CinderTransaction> GetTransactionByHash(string hash, CancellationToken cancellationToken = default);

        Task<IEnumerable<CinderTransaction>> GetTransactionByBlockHash(string blockHash,
            CancellationToken cancellationToken = default);
    }
}
