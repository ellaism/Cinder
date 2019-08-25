using System.Collections.Generic;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Events
{
    public class RecentTransactionsUpdatedEvent
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }
    }
}
