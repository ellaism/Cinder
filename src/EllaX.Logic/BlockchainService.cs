using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;
using EllaX.Logic.Clients;
using EllaX.Logic.Clients.Responses;
using EllaX.Logic.Clients.Responses.Parity.NetPeers;
using EllaX.Logic.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EllaX.Logic
{
    public class BlockchainService : Service, IBlockchainService
    {
        private readonly IBlockchainClient _blockchainClient;
        private readonly ILogger<BlockchainService> _logger;
        private readonly IMapper _mapper;

        public BlockchainService(IMediator eventBus, ILogger<BlockchainService> logger, IMapper mapper,
            IBlockchainClient blockchainClient) : base(eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _blockchainClient = blockchainClient;
        }

        public async Task GetHealthAsync(IList<string> hosts, CancellationToken ctx = default(CancellationToken))
        {
            List<Task<Response<NetPeerResult>>> tasks =
                hosts.Select(host => _blockchainClient.GetNetPeersAsync(host, ctx)).ToList();

            await Task.WhenAll(tasks.Select(async task => await ProcessNetPeerResult(task.Result, ctx)))
                .ConfigureAwait(false);
        }

        private async Task ProcessNetPeerResult(Response<NetPeerResult> response,
            CancellationToken ctx = default(CancellationToken))
        {
            if (!response.Result.Peers.Any())
            {
                return;
            }

            IEnumerable<Task> events = response.Result.Peers.Select(peer =>
                EventBus.Publish(new PeerNotification {Peer = _mapper.Map<Peer>(peer)}, ctx));

            await Task.WhenAll(events).ConfigureAwait(false);
        }
    }
}
