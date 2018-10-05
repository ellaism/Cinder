using System;
using EllaX.Logic.Services;

namespace EllaX.Logic.Factories
{
    public interface IBlockchainConnectionFactory
    {
        IBlockchainService CreateConnection(string url);
        IBlockchainService CreateConnection(Uri url);
    }
}
