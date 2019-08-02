using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class TransactionController : BaseController
    {
        [HttpGet("recent")]
        [ProducesResponseType(typeof(GetRecentTransactions.Model), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GetRecentTransactions.Model>> GetRecentAsync()
        {
            return await Mediator.Send(new GetRecentTransactions.Query());
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetTransactionByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetTransactionByHash.Model> GetBlockByHashAsync(string hash)
        {
            return await Mediator.Send(new GetTransactionByHash.Query {Hash = hash});
        }
    }
}
