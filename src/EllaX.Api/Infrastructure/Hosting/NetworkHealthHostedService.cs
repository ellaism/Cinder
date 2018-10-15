using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Hosting
{
    public class NetworkHealthHostedService : IHostedService
    {
        private readonly List<string> _hosts = new List<string>
        {
            "http://104.248.178.221:8545",
            "http://178.62.97.165:8545",
            "http://192.34.62.52:8545",
            "http://178.128.235.125:8545",
            "http://165.227.98.9:8545"
        };

        private readonly ILogger<NetworkHealthHostedService> _logger;

        private readonly IServiceProvider _services;

        public NetworkHealthHostedService(IServiceProvider services, ILogger<NetworkHealthHostedService> logger)
        {
            _services = services;
            _logger = logger;
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

            using (IServiceScope scope = _services.CreateScope())
            {
                IBlockchainService blockchainService = scope.ServiceProvider.GetRequiredService<IBlockchainService>();
                await blockchainService.GetHealthAsync(_hosts, cancellationToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
        }
    }
}
