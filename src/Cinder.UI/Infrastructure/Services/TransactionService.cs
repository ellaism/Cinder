using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Cinder.UI.Infrastructure.Services
{
    public class TransactionService : ServiceBase, ITransactionService
    {
        private readonly IMessageBus _bus;

        public TransactionService(ICacheClient cache, IMessageBus bus) : base(cache, nameof(BlockService))
        {
            _bus = bus;
        }

        public async Task UpdateRecentTransactions(IEnumerable<RecentTransactionDto> transactions)
        {
            List<RecentTransactionDto> enumerable = transactions.ToList();
            await Save(CacheKey.RecentTransactions, enumerable).ConfigureAwait(false);
            await _bus.PublishAsync(new RecentTransactionsUpdatedEvent {Transactions = enumerable}).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions()
        {
            IEnumerable<RecentTransactionDto> transactions =
                await Get<IEnumerable<RecentTransactionDto>>(CacheKey.RecentTransactions).ConfigureAwait(false);

            return transactions ?? new List<RecentTransactionDto>();
        }

        internal static class CacheKey
        {
            public const string RecentTransactions = "RecentTransactions";
        }
    }
}
