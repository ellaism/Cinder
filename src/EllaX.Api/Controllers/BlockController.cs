using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EllaX.Application.Features.Block;
using EllaX.Data.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    public class BlockController : BaseController
    {
        /// <summary>
        ///     Get a list of Blocks.
        /// </summary>
        /// <returns>A list of Blocks.</returns>
        /// <response code="200">A list of Blocks was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IPaginatedResult<List.Model>>> GetAsync([FromQuery] List.Query query)
        {
            IPaginatedResult<List.Model> models = await Mediator.Send(query);

            return Ok(models);
        }

        /// <summary>
        ///     Get a Block by Block Number.
        /// </summary>
        /// <returns>A Block.</returns>
        /// <response code="200">A Block was successfully retrieved.</response>
        [HttpGet("{blockNumber}", Name = nameof(GetByNumberAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Detail.Model>> GetByNumberAsync([FromRoute] [Required] ulong blockNumber)
        {
            Detail.Model model = await Mediator.Send(new Detail.Query {BlockNumber = blockNumber});

            return Ok(model);
        }
    }
}
