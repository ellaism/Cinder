using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task UpdateRecentTransactions(IEnumerable<RecentTransactionDto> transactions);
        Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions();
    }
}
