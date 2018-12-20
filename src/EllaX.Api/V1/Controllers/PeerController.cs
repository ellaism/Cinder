using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EllaX.Application.Peer;
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
        /// <returns>A list of peers.</returns>
        /// <response code="200">A list of Peers was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List.Model), StatusCodes.Status200OK)]
        public async Task<IEnumerable<List.Model>> GetAsync([FromQuery] List.Query query)
        {
            IEnumerable<List.Model> models = await _mediator.Send(query);

            return models;
        }

        /// <summary>
        ///     Get a Peer by ID.
        /// </summary>
        /// <returns>A Peer.</returns>
        /// <response code="200">A Peer was successfully retrieved.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Detail.Model), StatusCodes.Status200OK)]
        public async Task<ActionResult<Detail.Model>> GetAsync([FromRoute] [Required] string id)
        {
            Detail.Model model = await _mediator.Send(new Detail.Query {Id = id});

            return model;
        }

        /// <summary>
        ///     Create a Peer.
        /// </summary>
        /// <returns>A newly created Peer.</returns>
        /// <response code="201">A Peer was successfully created.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Create.Model), StatusCodes.Status201Created)]
        public async Task<ActionResult<Create.Model>> PostAsync([FromBody] Create.Command command)
        {
            Create.Model model = await _mediator.Send(command);

            return CreatedAtRoute(nameof(GetAsync), new {id = model.Id}, model);
        }

        /// <summary>
        ///     Update a Peer.
        /// </summary>
        /// <response code="204">A Peer was successfully updated.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Create.Model>> PutAsync([FromRoute] [Required] string id,
            [FromBody] Update.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        ///     Delete a Peer.
        /// </summary>
        /// <response code="204">A Peer was successfully deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Create.Model>> DeleteAsync([FromRoute] [Required] string id)
        {
            await _mediator.Send(new Delete.Command {Id = id});

            return NoContent();
        }
    }
}
