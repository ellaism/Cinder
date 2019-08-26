using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class BlockModel : PageComponent
    {
        public BlockDto Block { get; set; }
        public IEnumerable<TransactionDto> Transactions { get; set; }

        [Parameter]
        public string Hash { get; set; }

        [Inject]
        public IBlockService BlockService { get; set; }

        [Inject]
        public ITransactionService TransactionService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetLoading(true);
            Block = await BlockService.GetBlockByHash(Hash).ConfigureAwait(false);
            Transactions = await TransactionService.GetTransactionsByBlockHash(Block.Hash).ConfigureAwait(false);
            SetLoading(false);
        }
    }
}
