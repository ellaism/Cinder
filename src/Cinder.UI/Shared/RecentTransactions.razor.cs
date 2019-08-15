using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class RecentTransactionsModel : ComponentBase
    {
        public IEnumerable<RecentTransactionDto> Transactions;

        [Inject]
        protected ICinderApiService BlockchainService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await LoadTransactions();
        }

        private async Task LoadTransactions()
        {
            Transactions = await BlockchainService.GetRecentTransactions();
        }
    }
}
