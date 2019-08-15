using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface ICinderApiService
    {
        Task<IEnumerable<RecentBlockDto>> GetRecentBlocks();
        Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions();
    }
}
