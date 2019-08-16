using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cinder.UI.Infrastructure.Hosting
{
    public class StatsBackgroundService : BackgroundService
    {
        private readonly ICinderApiService _apiService;
        private readonly ILogger<StatsBackgroundService> _logger;
        private readonly IStatsService _stats;

        public StatsBackgroundService(ILogger<StatsBackgroundService> logger, ICinderApiService apiService, IStatsService stats)
        {
            _logger = logger;
            _apiService = apiService;
            _stats = stats;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting hosted service {Service}", nameof(StatsBackgroundService));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    IEnumerable<RecentBlockDto> blocks = await _apiService.GetRecentBlocks().ConfigureAwait(false);
                    await _stats.UpdateRecentBlocks(blocks).ConfigureAwait(false);

                    IEnumerable<RecentTransactionDto> transactions = await _apiService.GetRecentTransactions().ConfigureAwait(false);
                    await _stats.UpdateRecentTransactions(transactions).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "{Class} -> {Method} -> When getting recent blocks", nameof(StatsBackgroundService),
                        nameof(ExecuteAsync));

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken).ConfigureAwait(false);
                }

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
