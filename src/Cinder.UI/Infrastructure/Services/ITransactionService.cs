using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task UpdateRecentTransactions(IEnumerable<TransactionDto> transactions);
        Task<TransactionDto> GetTransactionByHash(string hash);
        Task<IEnumerable<TransactionDto>> GetRecentTransactions();
        Task<IEnumerable<TransactionDto>> GetTransactionsByBlockHash(string blockHash);
    }
}
