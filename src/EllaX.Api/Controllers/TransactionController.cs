using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Application.Features.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    public class TransactionController : BaseController
    {
        [HttpGet("recent")]
        [ProducesResponseType(typeof(GetRecentTransactions.Model), StatusCodes.Status200OK)]
        public async Task<IList<GetRecentTransactions.Model>> GetRecentAsync()
        {
            return await Mediator.Send(new GetRecentTransactions.Query());
        }
    }
}
