using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Stats;
using Cinder.Core.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class StatsController : BaseController
    {
        [HttpGet("richest")]
        [ProducesResponseType(typeof(GetRichest.Model), StatusCodes.Status200OK)]
        public async Task<IPage<GetRichest.Model>> GetRichest(int? page, int? size)
        {
            return await Mediator.Send(new GetRichest.Query {Page = page, Size = size}).ConfigureAwait(false);
        }
    }
}
