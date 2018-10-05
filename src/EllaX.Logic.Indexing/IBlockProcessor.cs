using System.Threading.Tasks;

namespace EllaX.Logic.Indexing
{
    public interface IBlockProcessor
    {
        Task ProcessBlockAsync(long blockNumber);
    }
}
