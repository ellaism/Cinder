using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface IBlockService
    {
        Task UpdateRecentBlocks(IEnumerable<RecentBlockDto> blocks);
        Task<IEnumerable<RecentBlockDto>> GetRecentBlocks();
        Task<BlockDto> GetBlockByHash(string hash);
        Task<BlockDto> GetBlockByNumber(ulong number);
    }
}
