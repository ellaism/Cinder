using System.Collections.Generic;
using System.Threading.Tasks;
using EllaX.Logic;
using Microsoft.AspNetCore.Mvc;

namespace EllaX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly IBlockchainService _blockchainService;

        private List<string> _hosts = new List<string>
        {
            "http://104.248.178.221:8545",
            "http://178.62.97.165:8545",
            "http://192.34.62.52:8545",
            "http://178.128.235.125:8545",
            "http://165.227.98.9:8545"
        };

        public NetworkController(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthAsync()
        {
            await _blockchainService.GetHealthAsync(_hosts);

            return Ok();
        }
    }
}
