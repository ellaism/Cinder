namespace Cinder.UI.Infrastructure.Components
{
    public abstract class PageComponent : EventBusComponent
    {
        public bool IsLoading { get; private set; }

        public void SetLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
