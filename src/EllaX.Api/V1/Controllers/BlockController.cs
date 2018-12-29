using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EllaX.Application.Features.Block;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Get a list of Blocks.
        /// </summary>
        /// <returns>A list of Blocks.</returns>
        /// <response code="200">A list of Blocks was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<List.Model>>> GetAsync([FromQuery] List.Query query)
        {
            IEnumerable<List.Model> models = await _mediator.Send(query);

            return Ok(models);
        }

        /// <summary>
        ///     Get a Block by Height.
        /// </summary>
        /// <returns>A Block.</returns>
        /// <response code="200">A Block was successfully retrieved.</response>
        [HttpGet("{height}", Name = nameof(GetByHeightAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Detail.Model>> GetByHeightAsync([FromRoute] [Required] string height)
        {
            Detail.Model model = await _mediator.Send(new Detail.Query {Height = height});

            return Ok(model);
        }
    }
}
