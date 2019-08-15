using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentBlocksModel : ComponentBase
    {
        public IEnumerable<RecentBlockDto> Blocks;

        [Inject]
        protected ICinderApiService BlockchainService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await LoadBlocks();
        }

        private async Task LoadBlocks()
        {
            Blocks = await BlockchainService.GetRecentBlocks();
        }
    }
}
