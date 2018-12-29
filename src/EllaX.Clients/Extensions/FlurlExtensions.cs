using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Clients.Responses;
using Flurl.Http;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions
{
    public static class FlurlExtensions
    {
        public static async Task<Response<TResult>> PostJsonAsync<TResult>(this IFlurlRequest request, object data,
            CancellationToken cancellationToken = default)
        {
            using (HttpResponseMessage response = await request.PostJsonAsync(data, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<Response<TResult>>(content);
            }
        }
    }
}
