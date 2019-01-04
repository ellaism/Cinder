using Nethereum.Web3;

namespace EllaX.Clients.Blockchain
{
    public interface IBlockchainClient
    {
        Web3 Web3 { get; }
    }
}
