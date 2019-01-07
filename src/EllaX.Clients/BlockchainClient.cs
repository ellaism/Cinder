using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Block = EllaX.Core.Entities.Block;

namespace EllaX.Clients
{
    public class BlockchainClient : IBlockchainClient
    {
        private readonly IMapper _mapper;

        public BlockchainClient(IOptions<BlockchainClientOptions> options, IMapper mapper)
        {
            _mapper = mapper;
            Web3 = new Web3(options.Value.Endpoint);
        }

        public Web3 Web3 { get; }

        public async Task<Block> GetBlockWithTransactionsAsync(uint blockNumber, CancellationToken cancellationToken = default)
        {
            BlockWithTransactions response =
                await Web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));
            cancellationToken.ThrowIfCancellationRequested();
            Block block = _mapper.Map<Block>(response);

            return block;
        }

        public async Task<uint> GetLatestBlockNumberAsync(CancellationToken cancellationToken = default)
        {
            HexBigInteger response = await Web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            return (uint) response.Value;
        }
    }
}
