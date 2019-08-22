using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.ellaism.io/");
            client.DefaultRequestHeaders.Add("User-Agent", $"Cinder.UI/{VersionInfo.Version}");
            _client = client;
        }

        public async Task<AddressDto> GetAddressByHash(string hash)
        {
            string url = $"/v1/address/{hash}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            AddressDto result = await response.Content.ReadAsAsync<AddressDto>().ConfigureAwait(false);

            return result;
        }

        public async Task<BlockDto> GetBlockByHash(string hash)
        {
            string url = $"/v1/block/{hash}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            BlockDto result = await response.Content.ReadAsAsync<BlockDto>().ConfigureAwait(false);

            return result;
        }

        public async Task<BlockDto> GetBlockByNumber(string number)
        {
            string url = $"/v1/block/height/{number}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            BlockDto result = await response.Content.ReadAsAsync<BlockDto>().ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<RecentBlockDto>> GetRecentBlocks(int? limit = null)
        {
            string url = "/v1/block/recent";

            if (limit.HasValue)
            {
                url += $"?limit={limit.Value}";
            }

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            IEnumerable<RecentBlockDto> result =
                await response.Content.ReadAsAsync<IEnumerable<RecentBlockDto>>().ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions(int? limit = null)
        {
            string url = "/v1/transaction/recent";

            if (limit.HasValue)
            {
                url += $"?limit={limit.Value}";
            }

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            IEnumerable<RecentTransactionDto> result =
                await response.Content.ReadAsAsync<IEnumerable<RecentTransactionDto>>().ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByBlockHash(string blockHash)
        {
            string url = $"/v1/transaction/block/{blockHash}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            IEnumerable<TransactionDto> result =
                await response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().ConfigureAwait(false);

            return result;
        }
    }
}
