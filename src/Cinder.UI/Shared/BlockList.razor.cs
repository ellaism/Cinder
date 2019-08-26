using System.Collections.Generic;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class BlockListModel : EventBusComponent
    {
        [Parameter]
        public IEnumerable<BlockDto> Blocks { get; set; }
    }
}
