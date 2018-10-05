using System;
using EllaX.Logic.Services;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Factories
{
    public class BlockchainConnectionFactory : IBlockchainConnectionFactory
    {
        private readonly ILogger<BlockchainService> _logger;

        public BlockchainConnectionFactory(ILogger<BlockchainService> logger)
        {
            _logger = logger;
        }

        public IBlockchainService CreateConnection(string url)
        {
            return new BlockchainService(_logger, url);
        }

        public IBlockchainService CreateConnection(Uri url)
        {
            return new BlockchainService(_logger, url);
        }
    }
}
