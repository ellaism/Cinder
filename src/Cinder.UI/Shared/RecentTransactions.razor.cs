using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Cinder.UI.Infrastructure.Services;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentTransactionsModel : CinderComponentBase
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }

        [Inject]
        public IMessageBus Bus { get; set; }

        [Inject]
        public ITransactionService Stats { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            SetLoading(true);
            Transactions = await Stats.GetRecentTransactions().ConfigureAwait(false);
            await Bus.SubscribeAsync<RecentTransactionsUpdatedEvent>(RecentTransactionsUpdatedEventHandler).ConfigureAwait(false);
            SetLoading(false);
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
