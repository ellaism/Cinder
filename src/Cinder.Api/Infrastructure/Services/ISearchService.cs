using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Models;

namespace Cinder.Api.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<SearchResult> ExecuteSearch(string query);
    }
}
