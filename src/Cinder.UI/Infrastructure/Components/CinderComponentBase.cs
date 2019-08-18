using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Infrastructure.Components
{
    public abstract class CinderComponentBase : ComponentBase
    {
        public bool IsLoading { get; private set; }

        public void SetLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
