using System;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Paging;
using Foundatio.Caching;

namespace Cinder.UI.Infrastructure.Services
{
    public class AddressService : ServiceBase, IAddressService
    {
        private readonly IApiClient _api;

        public AddressService(IApiClient api, ICacheClient cache) : base(cache, nameof(AddressService))
        {
            _api = api;
        }

        public async Task<AddressDto> GetAddressByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 42)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            string key = $"{CacheKey.Address}{hash}";

            AddressDto address;
            if (await Exists(key).ConfigureAwait(false))
            {
                address = await Get<AddressDto>(key).ConfigureAwait(false);
            }
            else
            {
                address = await _api.GetAddressByHash(hash).ConfigureAwait(false);
                await Save(key, address, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return address;
        }

        public async Task<IPage<TransactionDto>> GetTransactionsAddressByHash(string hash, int? page, int? size)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 42)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            string key = $"{CacheKey.AddressTransaction}{hash}";

            IPage<TransactionDto> transactions;
            if (await Exists(key).ConfigureAwait(false))
            {
                transactions = await Get<IPage<TransactionDto>>(key).ConfigureAwait(false);
            }
            else
            {
                transactions = await _api.GetTransactionsByAddressHash(hash, page, size).ConfigureAwait(false);
                await Save(key, transactions, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return transactions;
        }

        internal static class CacheKey
        {
            public const string Address = "Address";
            public const string AddressTransaction = "AddressTransaction";
        }
    }
}
