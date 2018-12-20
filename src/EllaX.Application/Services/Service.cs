using MediatR;

namespace EllaX.Application.Services
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
