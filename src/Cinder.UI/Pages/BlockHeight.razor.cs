using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class BlockHeightModel : CinderComponentBase
    {
        public BlockDto Block;

        [Parameter]
        public ulong Number { get; set; }

        [Inject]
        public IBlockService BlockService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            SetLoading(true);
            Block = await BlockService.GetBlockByNumber(Number).ConfigureAwait(false);
            SetLoading(false);
        }
    }
}
