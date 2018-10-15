using System.Collections.Generic;
using EllaX.Api.Infrastructure;
using EllaX.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly InMemoryStatistics _statistics;

        public NetworkController(InMemoryStatistics statistics)
        {
            _statistics = statistics;
        }

        [HttpGet("health")]
        public IEnumerable<Peer> GetHealthAsync()
        {
            return _statistics.GetPeers();
        }
    }
}
