using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface IBlockService
    {
        Task UpdateRecentBlocks(IEnumerable<BlockDto> blocks);
        Task<IEnumerable<BlockDto>> GetRecentBlocks();
        Task<BlockDto> GetBlockByHash(string hash);
        Task<BlockDto> GetBlockByNumber(string number);
    }
}
