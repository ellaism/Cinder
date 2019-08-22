using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class SearchController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(SearchAll.Model), StatusCodes.Status200OK)]
        public async Task<SearchAll.Model> GetAddressByHashAsync(string query)
        {
            return await Mediator.Send(new SearchAll.Query {Q = query});
        }
    }
}
