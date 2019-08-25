using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class TransactionModel : CinderComponentBase
    {
        public TransactionDto Transaction { get; set; }

        [Parameter]
        public string Hash { get; set; }

        [Inject]
        public ITransactionService TransactionService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            SetLoading(true);
            Transaction = await TransactionService.GetTransactionByHash(Hash).ConfigureAwait(false);
            SetLoading(false);
        }
    }
}
