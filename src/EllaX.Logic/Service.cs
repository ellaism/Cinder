using MediatR;

namespace EllaX.Logic
{
    public abstract class Service
    {
        protected readonly IMediator EventBus;

        protected Service(IMediator eventBus)
        {
            EventBus = eventBus;
        }
    }
}
