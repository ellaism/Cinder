using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class AddressController : BaseController
    {
        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetAddressByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetAddressByHash.Model> GetAddressByHash(string hash)
        {
            return await Mediator.Send(new GetAddressByHash.Query {Hash = hash});
        }
    }
}
