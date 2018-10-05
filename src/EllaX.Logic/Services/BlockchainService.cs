using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;

namespace EllaX.Logic.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly Web3 _connection;
        private readonly ILogger<BlockchainService> _logger;

        public BlockchainService(ILogger<BlockchainService> logger, string url)
        {
            _logger = logger;
            _connection = new Web3(url);
        }

        public BlockchainService(ILogger<BlockchainService> logger, Uri url)
        {
            _connection = new Web3(url.ToString());
        }

        public async Task<decimal> GetBalanceAsync(string address)
        {
            HexBigInteger balance = await _connection.Eth.GetBalance.SendRequestAsync(address);

            return UnitConversion.Convert.FromWei(balance);
        }
    }
}
