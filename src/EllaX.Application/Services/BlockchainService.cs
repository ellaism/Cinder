using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EllaX.Application.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IBlockchainClient _blockchainClient;
        private readonly ILogger<BlockchainService> _logger;
        private readonly IMapper _mapper;
        private readonly INetworkClient _networkClient;

        public BlockchainService(ILogger<BlockchainService> logger, IMapper mapper, IBlockchainClient blockchainClient,
            INetworkClient networkClient)
        {
            _logger = logger;
            _mapper = mapper;
            _blockchainClient = blockchainClient;
            _networkClient = networkClient;
        }

        public async Task<IReadOnlyCollection<PeerItem>> GetPeersAsync(CancellationToken cancellationToken = default)
        {
            List<Task<Response<NetPeerResult>>> tasks = _networkClient.Nodes
                .Select(node => _networkClient.GetNetPeersAsync(node, cancellationToken)).ToList();
            List<PeerItem> results = new List<PeerItem>();

            await Task.Factory.ContinueWhenAll(tasks.ToArray(), task =>
            {
                foreach (Task<Response<NetPeerResult>> t in task)
                {
                    if (t.IsFaulted)
                    {
                        _logger.LogError(t.Exception, $"{nameof(BlockchainService)} -> {nameof(GetPeersAsync)}");

                        continue;
                    }

                    if (!t.IsCompleted)
                    {
                        continue;
                    }

                    results.AddRange(t.Result.Result.Peers.Where(peer => peer.Protocols.Eth != null));
                }
            });

            return results;
        }
    }
}
