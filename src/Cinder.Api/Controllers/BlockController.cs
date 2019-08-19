using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Block;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class BlockController : BaseController
    {
        [HttpGet("recent")]
        [ProducesResponseType(typeof(GetRecentBlocks.Model), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GetRecentBlocks.Model>> GetRecentAsync([FromQuery] int? limit = null)
        {
            return await Mediator.Send(new GetRecentBlocks.Query {Limit = limit});
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetBlockByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetBlockByHash.Model> GetBlockByHashAsync([FromRoute] string hash)
        {
            return await Mediator.Send(new GetBlockByHash.Query {Hash = hash});
        }

        [HttpGet("{number}")]
        [ProducesResponseType(typeof(GetBlockByNumber.Model), StatusCodes.Status200OK)]
        public async Task<GetBlockByNumber.Model> GetBlockByNumberAsync([FromRoute] ulong number)
        {
            return await Mediator.Send(new GetBlockByNumber.Query {Number = number});
        }
    }
}
