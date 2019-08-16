using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Foundatio.Messaging;
using Microsoft.Extensions.Caching.Memory;

namespace Cinder.UI.Infrastructure.Services
{
    public class StatsService : IStatsService
    {
        public const string CacheKeyRecentBlocks = "StatsService.RecentBlocks";
        public const string CacheKeyRecentTransactions = "StatsService.RecentTransactions";
        private readonly IMessageBus _bus;
        private readonly IMemoryCache _cache;

        public StatsService(IMemoryCache cache, IMessageBus bus)
        {
            _cache = cache;
            _bus = bus;
        }

        public async Task UpdateRecentBlocks(IEnumerable<RecentBlockDto> blocks)
        {
            using ICacheEntry entry = _cache.CreateEntry(CacheKeyRecentBlocks);
            entry.Value = blocks;
            entry.SetPriority(CacheItemPriority.NeverRemove);

            await _bus.PublishAsync(new RecentBlocksUpdatedEvent {Blocks = blocks}).ConfigureAwait(false);
        }

        public Task<IEnumerable<RecentBlockDto>> GetRecentBlocks()
        {
            if (_cache.TryGetValue(CacheKeyRecentBlocks, out IEnumerable<RecentBlockDto> blocks))
            {
                return Task.FromResult(blocks);
            }

            return Task.FromResult((IEnumerable<RecentBlockDto>) new List<RecentBlockDto>());
        }

        public async Task UpdateRecentTransactions(IEnumerable<RecentTransactionDto> transactions)
        {
            using ICacheEntry entry = _cache.CreateEntry(CacheKeyRecentTransactions);
            entry.Value = transactions;
            entry.SetPriority(CacheItemPriority.NeverRemove);

            await _bus.PublishAsync(new RecentTransactionsUpdatedEvent {Transactions = transactions}).ConfigureAwait(false);
        }

        public Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions()
        {
            if (_cache.TryGetValue(CacheKeyRecentTransactions, out IEnumerable<RecentTransactionDto> transactions))
            {
                return Task.FromResult(transactions);
            }

            return Task.FromResult((IEnumerable<RecentTransactionDto>) new List<RecentTransactionDto>());
        }
    }
}
