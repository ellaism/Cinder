using System.Threading.Tasks;
using EllaX.Application.Features.Peer;
using EllaX.Data.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class PeerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Get a list of Peers.
        /// </summary>
        /// <returns>A list of Peers.</returns>
        /// <response code="200">A list of Peers was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IPaginatedResult<List.Model>>> GetAsync([FromQuery] List.Query query)
        {
            IPaginatedResult<List.Model> models = await _mediator.Send(query);

            return Ok(models);
        }
    }
}
