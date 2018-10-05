using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Newtonsoft.Json.Linq;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface ITransactionVmStackRepository
    {
        Task UpsertAsync(string transactionHash, string address, JObject stackTrace);
        Task<ITransactionVmStackView> FindByTransactionHashAync(string hash);
        Task<ITransactionVmStackView> FindByAddressAndTransactionHashAync(string address, string hash);
    }
}
