using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Application.Features.Block;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    public class BlockController : BaseController
    {
        [HttpGet("recent")]
        [ProducesResponseType(typeof(GetRecentBlocks.Model), StatusCodes.Status200OK)]
        public async Task<IList<GetRecentBlocks.Model>> GetRecentAsync()
        {
            return await Mediator.Send(new GetRecentBlocks.Query());
        }
    }
}
