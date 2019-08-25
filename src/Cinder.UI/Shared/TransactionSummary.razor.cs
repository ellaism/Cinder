using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class TransactionSummaryModel : CinderComponentBase
    {
        [Parameter]
        public string BlockHash { get; set; }

        public IEnumerable<TransactionDto> Transactions { get; set; }

        [Inject]
        public ITransactionService TransactionService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            SetLoading(true);
            Transactions = await TransactionService.GetTransactionsByBlockHash(BlockHash).ConfigureAwait(false);
            SetLoading(false);
        }
    }
}
