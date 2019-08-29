using Cinder.UI.Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Shared
{
    public class AddressDetailModel : ComponentBase
    {
        [Parameter]
        public AddressDto Address { get; set; }
    }
}
