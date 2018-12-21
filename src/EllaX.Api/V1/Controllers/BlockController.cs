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

        /// <summary>
        ///     Create a Block.
        /// </summary>
        /// <returns>A newly created Block.</returns>
        /// <response code="201">A Block was successfully created.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Create.Model>> PostAsync([FromBody] Create.Command command)
        {
            Create.Model model = await _mediator.Send(command);

            return CreatedAtRoute(nameof(GetByHeightAsync), new {height = model.Height}, model);
        }

        /// <summary>
        ///     Update a Block.
        /// </summary>
        /// <response code="204">A Block was successfully updated.</response>
        [HttpPut("{height}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Create.Model>> PutAsync([FromRoute] [Required] string height,
            [FromBody] Update.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        ///     Delete a Block.
        /// </summary>
        /// <response code="204">A Block was successfully deleted.</response>
        [HttpDelete("{height}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Create.Model>> DeleteAsync([FromRoute] [Required] string height)
        {
            await _mediator.Send(new Delete.Command {Height = height});

            return NoContent();
        }
    }
}
