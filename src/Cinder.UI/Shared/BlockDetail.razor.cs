using Cinder.UI.Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class BlockDetailModel : ComponentBase
    {
        [Parameter]
        public BlockDto Block { get; set; }
    }
}
