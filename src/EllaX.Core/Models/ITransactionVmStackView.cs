using Newtonsoft.Json.Linq;

namespace EllaX.Core.Models
{
    public interface ITransactionVmStackView
    {
        string Address { get; }
        string StructLogs { get; }
        string TransactionHash { get; }
        JArray GetStructLogs();
    }
}
