using System.Threading.Tasks;
using EllaX.Core.Dtos;
using EllaX.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public NetworkController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        ///     Gets the network health.
        /// </summary>
        /// <returns>The network health.</returns>
        /// <response code="200">The network health was successfully retrieved.</response>
        [HttpGet("health")]
        [ProducesResponseType(typeof(NetworkHealthResultDto), 200)]
        public async Task<NetworkHealthResultDto> GetHealthAsync()
        {
            NetworkHealthResultDto response = await _statisticsService.GetNetworkHealthAsync<NetworkHealthResultDto>();

            return response;
        }
    }
}
