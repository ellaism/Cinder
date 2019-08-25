using System;
using System.Threading;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Paging;
using Cinder.UI.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cinder.UI.Infrastructure.Hosting
{
    public class StatsBackgroundService : BackgroundService
    {
        private readonly IApiClient _apiService;
        private readonly IBlockService _blockService;
        private readonly ILogger<StatsBackgroundService> _logger;
        private readonly ITransactionService _transactionService;

        public StatsBackgroundService(ILogger<StatsBackgroundService> logger, IApiClient apiService, IBlockService blockService,
            ITransactionService transactionService)
        {
            _logger = logger;
            _apiService = apiService;
            _blockService = blockService;
            _transactionService = transactionService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting hosted service {Service}", nameof(StatsBackgroundService));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    IPage<BlockDto> blocks = await _apiService.GetBlocks(1, 20).ConfigureAwait(false);
                    await _blockService.UpdateRecentBlocks(blocks.Items).ConfigureAwait(false);

                    IPage<TransactionDto> transactions = await _apiService.GetTransactions(1, 20).ConfigureAwait(false);
                    await _transactionService.UpdateRecentTransactions(transactions.Items).ConfigureAwait(false);
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
