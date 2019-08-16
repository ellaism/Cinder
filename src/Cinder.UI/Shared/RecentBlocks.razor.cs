using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentBlocksModel : ComponentBase
    {
        public IEnumerable<RecentBlockDto> Blocks;

        [Inject]
        public IMessageBus Bus { get; set; }

        [Inject]
        public IStatsService Stats { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Blocks = await Stats.GetRecentBlocks().ConfigureAwait(false);
            await Bus.SubscribeAsync<RecentBlocksUpdatedEvent>(RecentBlocksUpdatedEventHandler).ConfigureAwait(false);
        }

        private async Task RecentBlocksUpdatedEventHandler(RecentBlocksUpdatedEvent message)
        {
            await InvokeAsync(() =>
            {
                Blocks = message.Blocks;
                StateHasChanged();
            }).ConfigureAwait(false);
        }
    }
}
