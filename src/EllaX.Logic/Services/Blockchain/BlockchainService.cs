using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Network;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Parity.NetPeers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Services.Blockchain
{
    public class BlockchainService : Service, IBlockchainService
    {
        private readonly IBlockchainClient _blockchainClient;
        private readonly ILogger<BlockchainService> _logger;
        private readonly IMapper _mapper;
        private readonly INetworkClient _networkClient;

        public BlockchainService(IMediator eventBus, ILogger<BlockchainService> logger, IMapper mapper,
            IBlockchainClient blockchainClient, INetworkClient networkClient) : base(eventBus)
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
