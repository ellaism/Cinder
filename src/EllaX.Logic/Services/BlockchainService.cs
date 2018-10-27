using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Network;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Parity.NetPeers;
using EllaX.Core.Entities;
using EllaX.Logic.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic.Services
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

        public async Task GetHealthAsync(CancellationToken cancellationToken = default)
        {
            List<Task<Response<NetPeerResult>>> tasks = _networkClient.Nodes
                .Select(node => _networkClient.GetNetPeersAsync(node, cancellationToken)).ToList();

            await Task.Factory.ContinueWhenAll(tasks.ToArray(), async task =>
            {
                foreach (Task<Response<NetPeerResult>> t in task)
                {
                    if (t.IsFaulted)
                    {
                        _logger.LogError(t.Exception, "BlockchainService -> GetNetworkHealthAsync");
                        continue;
                    }

                    if (!t.IsCompleted)
                    {
                        continue;
                    }

                    await ProcessNetPeerResultAsync(t.Result, cancellationToken);
                }
            });
        }

        private async Task ProcessNetPeerResultAsync(Response<NetPeerResult> response,
            CancellationToken cancellationToken = default)
        {
            if (!response.Result.Peers.Any())
            {
                return;
            }

            IEnumerable<Task> events = response.Result.Peers.Where(peer => peer.Protocols.Eth != null).Select(peer =>
                EventBus.Publish(new PeerNotification {Peer = _mapper.Map<Peer>(peer)}, cancellationToken));

            await Task.WhenAll(events).ConfigureAwait(false);
        }
    }
}
