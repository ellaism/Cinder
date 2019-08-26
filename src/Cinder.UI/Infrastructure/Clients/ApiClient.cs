using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Paging;

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

        public async Task<IPage<BlockDto>> GetBlocks(int? page, int? size, SortOrder sort = SortOrder.Ascending)
        {
            string url = $"/v1/block?page={page}&size={size}&sort={sort}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            PagedEnumerable<BlockDto> result =
                await response.Content.ReadAsAsync<PagedEnumerable<BlockDto>>().ConfigureAwait(false);

            return result;
        }

        public async Task<IPage<TransactionDto>> GetTransactions(int? page, int? size, SortOrder sort = SortOrder.Ascending)
        {
            string url = $"/v1/transaction?page={page}&size={size}&sort={sort}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            PagedEnumerable<TransactionDto> result =
                await response.Content.ReadAsAsync<PagedEnumerable<TransactionDto>>().ConfigureAwait(false);

            return result;
        }

        public async Task<TransactionDto> GetTransactionByHash(string hash)
        {
            string url = $"/v1/transaction/{hash}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            TransactionDto result = await response.Content.ReadAsAsync<TransactionDto>().ConfigureAwait(false);

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

        public async Task<SearchResultDto> Search(string query)
        {
            string url = $"/v1/search?query={query}";

            using HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            SearchResultDto result = await response.Content.ReadAsAsync<SearchResultDto>().ConfigureAwait(false);

            return result;
        }
    }
}
