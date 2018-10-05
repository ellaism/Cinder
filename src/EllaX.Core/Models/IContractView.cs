namespace EllaX.Core.Models
{
    public interface IContractView
    {
        string Abi { get; }
        string Address { get; }
        string Code { get; }
        string Creator { get; }
        string Name { get; }
        string TransactionHash { get; }
    }
}
