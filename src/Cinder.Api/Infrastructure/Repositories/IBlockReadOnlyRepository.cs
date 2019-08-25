using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Documents;

namespace Cinder.Api.Infrastructure.Repositories
{
    public interface IBlockReadOnlyRepository
    {
        Task<IPage<CinderBlock>> GetBlocks(int? page = null, int? size = null, SortOrder sort = SortOrder.Ascending,
            CancellationToken cancellationToken = default);

        Task<CinderBlock> GetBlockByHash(string hash, CancellationToken cancellationToken = default);
        Task<CinderBlock> GetBlockByNumber(ulong number, CancellationToken cancellationToken = default);
    }
}
