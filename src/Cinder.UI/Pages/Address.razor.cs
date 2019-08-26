using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Pages
{
    public class AddressModel : PageComponent
    {
        public AddressDto Address { get; set; }

        [Parameter]
        public string Hash { get; set; }

        [Inject]
        public IAddressService AddressService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetLoading(true);
            Address = await AddressService.GetAddressByHash(Hash).ConfigureAwait(false);
            SetLoading(false);
        }
    }
}
