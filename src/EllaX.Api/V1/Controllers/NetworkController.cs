using System.Threading.Tasks;
using EllaX.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        /// <summary>
        ///     Gets the network health.
        /// </summary>
        /// <returns>The network health.</returns>
        /// <response code="200">The network health was successfully retrieved.</response>
        [HttpGet("health")]
        [ProducesResponseType(typeof(NetworkHealthResultDto), 200)]
        public async Task<NetworkHealthResultDto> GetHealthAsync()
        {
            return await Task.FromResult(new NetworkHealthResultDto());
        }
    }
}
