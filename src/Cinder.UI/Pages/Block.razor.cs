using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class BlockModel : ComponentBase
    {
        [Parameter]
        public string Hash { get; set; }
    }
}
