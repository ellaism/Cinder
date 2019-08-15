using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Services
{
    public class CinderApiService : ICinderApiService
    {
        private readonly HttpClient _client;

        public CinderApiService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Add("User-Agent", "Cinder-UI");
            _client = client;
        }

        public async Task<IEnumerable<RecentBlockDto>> GetRecentBlocks()
        {
            HttpResponseMessage response = await _client.GetAsync("/v1/block/recent");
            response.EnsureSuccessStatusCode();

            IEnumerable<RecentBlockDto> result = await response.Content.ReadAsAsync<IEnumerable<RecentBlockDto>>();

            return result;
        }

        public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactions()
        {
            HttpResponseMessage response = await _client.GetAsync("/v1/transaction/recent");
            response.EnsureSuccessStatusCode();

            IEnumerable<RecentTransactionDto> result = await response.Content.ReadAsAsync<IEnumerable<RecentTransactionDto>>();

            return result;
        }
    }
}
