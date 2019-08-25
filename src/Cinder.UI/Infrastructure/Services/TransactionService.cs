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

        public TransactionService(ICacheClient cache, IMessageBus bus, IApiClient api) : base(cache, nameof(TransactionService))
        {
            _bus = bus;
            _api = api;
        }

        public async Task UpdateRecentTransactions(IEnumerable<TransactionDto> transactions)
        {
            List<TransactionDto> enumerable = transactions.ToList();
            await Save(CacheKey.Recent, enumerable).ConfigureAwait(false);
            await _bus.PublishAsync(new RecentTransactionsUpdatedEvent {Transactions = enumerable}).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TransactionDto>> GetRecentTransactions()
        {
            IEnumerable<TransactionDto> transactions =
                await Get<IEnumerable<TransactionDto>>(CacheKey.Recent).ConfigureAwait(false);

            return transactions ?? new List<TransactionDto>();
        }

        public async Task<TransactionDto> GetTransactionByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 66)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            string key = $"{CacheKey.Transaction}{hash}";

            TransactionDto transaction;
            if (await Exists(key).ConfigureAwait(false))
            {
                transaction = await Get<TransactionDto>(key).ConfigureAwait(false);
            }
            else
            {
                transaction = await _api.GetTransactionByHash(hash).ConfigureAwait(false);
                await Save(key, transaction, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return transaction;
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
            public const string Transaction = "Transaction";
            public const string Transactions = "Transactions";
        }
    }
}
