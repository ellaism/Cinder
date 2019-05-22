using System.Threading.Tasks;
using EllaX.Application.Features.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    public class AddressController : BaseController
    {
        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetAddressByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetAddressByHash.Model> GetAsync([FromQuery] GetAddressByHash.Query query)
        {
            return await Mediator.Send(query);
        }
    }
}
