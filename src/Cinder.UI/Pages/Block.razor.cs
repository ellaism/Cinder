using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class BlockModel : ComponentBase
    {
        public BlockDto Block;

        [Parameter]
        public string Hash { get; set; }

        [Inject]
        public IBlockService BlockService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Block = await BlockService.GetBlockByHash(Hash).ConfigureAwait(false);
        }
    }
}
