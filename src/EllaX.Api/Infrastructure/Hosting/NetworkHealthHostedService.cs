using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class NetworkHealthHostedService : IHostedService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IList<string> _hosts;
        private readonly ILogger<NetworkHealthHostedService> _logger;

        public NetworkHealthHostedService(IBlockchainService blockchainService, IConfiguration configuration,
            ILogger<NetworkHealthHostedService> logger)
        {
            _blockchainService = blockchainService;
            _logger = logger;
            _hosts = configuration.GetSection("Network:HealthNodes").Get<IList<string>>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            _logger.LogInformation("Network health hosted service is starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                await CheckNetworkHealthAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Network health hosted service is stopping");

            return Task.CompletedTask;
        }

        private async Task CheckNetworkHealthAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _blockchainService.GetHealthAsync(_hosts, cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
        }
    }
}
