using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Core.Dtos;
using EllaX.Logic;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public NetworkController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("health")]
        public async Task<IEnumerable<HealthDto>> GetHealthAsync()
        {
            return await _statisticsService.GetHealthAsync<HealthDto>();
        }
    }
}
