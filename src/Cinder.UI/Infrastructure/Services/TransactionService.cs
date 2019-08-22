using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Cinder.UI.Infrastructure.Services
{
    public class TransactionService : ServiceBase, ITransactionService
    {
        private readonly IApiClient _api;
        private readonly IMessageBus _bus;

        public TransactionService(ICacheClient cache, IMessageBus bus, IApiClient api) : base(cache, nameof(BlockService))
        {
            _bus = bus;
            _api = api;
        }

        public async Task UpdateRecentTransactions(IEnumerable<RecentTransactionDto> transactions)
        {
            List<RecentTransactionDto> enumerable = transactions.ToList();
            await Save(CacheKey.Recent, enumerable).ConfigureAwait(false);
            await _bus.PublishAsync(new RecentTransactionsUpdatedEvent {Transactions = enumerable}).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions()
        {
            IEnumerable<RecentTransactionDto> transactions =
                await Get<IEnumerable<RecentTransactionDto>>(CacheKey.Recent).ConfigureAwait(false);

            return transactions ?? new List<RecentTransactionDto>();
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByBlockHash(string blockHash)
        {
            if (string.IsNullOrEmpty(blockHash) || blockHash.Length != 66)
            {
                throw new ArgumentOutOfRangeException(nameof(blockHash));
            }

            string key = $"{CacheKey.Transactions}{blockHash}";

            IEnumerable<TransactionDto> transactions;
            if (await Exists(key).ConfigureAwait(false))
            {
                transactions = await Get<IEnumerable<TransactionDto>>(key).ConfigureAwait(false);
            }
            else
            {
                transactions = await _api.GetTransactionsByBlockHash(blockHash).ConfigureAwait(false);
                await Save(key, transactions, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return transactions;
        }

        internal static class CacheKey
        {
            public const string Recent = "Recent";
            public const string Transactions = "Transactions";
        }
    }
}
