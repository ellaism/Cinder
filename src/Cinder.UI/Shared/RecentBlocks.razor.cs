using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentBlocksModel : CinderComponentBase, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IEnumerable<RecentBlockDto> Blocks { get; set; }

        [Inject]
        public IMessageBus Bus { get; set; }

        [Inject]
        public IBlockService BlockService { get; set; }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        protected override async Task OnParametersSetAsync()
        {
            SetLoading(true);
            Blocks = await BlockService.GetRecentBlocks().ConfigureAwait(false);
            await Bus.SubscribeAsync<RecentBlocksUpdatedEvent>(RecentBlocksUpdatedEventHandler, _cancellationTokenSource.Token)
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
