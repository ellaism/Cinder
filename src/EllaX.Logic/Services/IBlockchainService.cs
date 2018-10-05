using System.Threading.Tasks;

namespace EllaX.Logic.Services
{
    public interface IBlockchainService
    {
        Task<decimal> GetBalanceAsync(string address);
    }
}
