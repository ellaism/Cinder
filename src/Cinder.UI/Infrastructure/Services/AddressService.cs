using System;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Dtos;
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

        internal static class CacheKey
        {
            public const string Address = "Address";
        }
    }
}
