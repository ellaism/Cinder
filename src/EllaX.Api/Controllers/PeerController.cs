using System.Threading.Tasks;
using EllaX.Application.Features.Peer;
using EllaX.Data.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    public class PeerController : BaseController
    {
        /// <summary>
        ///     Get a list of Peers.
        /// </summary>
        /// <returns>A list of Peers.</returns>
        /// <response code="200">A list of Peers was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IPaginatedResult<List.Model>>> GetAsync([FromQuery] List.Query query)
        {
            IPaginatedResult<List.Model> models = await Mediator.Send(query);

            return Ok(models);
        }
    }
}
