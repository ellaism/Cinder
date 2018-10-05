using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Newtonsoft.Json.Linq;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface ITransactionLogRepository
    {
        Task UpsertAsync(string transactionHash, long logIndex, JObject log);
        Task<ITransactionLogView> FindByTransactionHashAndLogIndexAsync(string hash, long idx);
    }
}
