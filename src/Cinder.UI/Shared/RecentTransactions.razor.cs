using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentTransactionsModel : ComponentBase
    {
        public IEnumerable<RecentTransactionDto> Transactions;

        [Inject]
        public IMessageBus Bus { get; set; }

        [Inject]
        public IStatsService Stats { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Transactions = await Stats.GetRecentTransactions().ConfigureAwait(false);
            await Bus.SubscribeAsync<RecentTransactionsUpdatedEvent>(RecentTransactionsUpdatedEventHandler).ConfigureAwait(false);
        }

        private async Task RecentTransactionsUpdatedEventHandler(RecentTransactionsUpdatedEvent message)
        {
            await InvokeAsync(() =>
                {
                    Transactions = message.Transactions;
                    StateHasChanged();
                })
                .ConfigureAwait(false);
        }
    }
}
