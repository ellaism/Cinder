using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Block;
using Cinder.Core.Paging;
using Cinder.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class BlockController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetBlocks.Model), StatusCodes.Status200OK)]
        public async Task<IPage<GetBlocks.Model>> GetBlocks(int? page, int? size, SortOrder sort = SortOrder.Ascending)
        {
            return await Mediator.Send(new GetBlocks.Query {Page = page, Size = size, Sort = sort}).ConfigureAwait(false);
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetBlockByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetBlockByHash.Model> GetBlockByHash([FromRoute] string hash)
        {
            return await Mediator.Send(new GetBlockByHash.Query {Hash = hash});.ConfigureAwait(false)
        }

        [HttpGet("height/{number}")]
        [ProducesResponseType(typeof(GetBlockByNumber.Model), StatusCodes.Status200OK)]
        public async Task<GetBlockByNumber.Model> GetBlockByNumber([FromRoute] ulong number)
        {
            return await Mediator.Send(new GetBlockByNumber.Query {Number = number}).ConfigureAwait(false);
        }
    }
}
