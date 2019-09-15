using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class SearchController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetResultsByQuery.Model), StatusCodes.Status200OK)]
        public async Task<GetResultsByQuery.Model> GetResultsByQuery(string query)
        {
            return await Mediator.Send(new GetResultsByQuery.Query {Q = query}).ConfigureAwait(false);
        }
    }
}
