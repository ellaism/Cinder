using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class IndexModel : PageComponent
    {
        [Parameter]
        public IEnumerable<BlockDto> Blocks { get; set; }

        [Inject]
        public IBlockService BlockService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetLoading(true);
            Blocks = await BlockService.GetRecentBlocks().ConfigureAwait(false);
            await Bus.SubscribeAsync<RecentBlocksUpdatedEvent>(RecentBlocksUpdatedEventHandler,
                    ComponentLifetimeCancellationTokenSource.Token)
                .ConfigureAwait(false);
            SetLoading(false);
        }

        private async Task RecentBlocksUpdatedEventHandler(RecentBlocksUpdatedEvent message)
        {
            await InvokeAsync(() =>
                {
                    Blocks = message.Blocks;
                    StateHasChanged();
                })
                .ConfigureAwait(false);
        }
    }
}
