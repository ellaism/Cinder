using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Indexer.Infrastructure.Clients.Responses;
using Newtonsoft.Json;

namespace EllaX.Indexer.Infrastructure.Clients
{
    public abstract class Client
    {
        private readonly HttpClient _client;

        protected Client(HttpClient client)
        {
            _client = client;
        }

        protected virtual HttpRequestMessage CreateRequest(Message message, string host)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, host)
            {
                Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json")
            };

            return request;
        }

        protected virtual async Task<Response<TResult>> SendAsync<TResult>(HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            using (HttpResponseMessage response = await _client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<Response<TResult>>(content);
            }
        }
    }
}
