using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class TransactionDetailModel : EventBusComponent
    {
        [Parameter]
        public TransactionDto Transaction { get; set; }
    }
}
