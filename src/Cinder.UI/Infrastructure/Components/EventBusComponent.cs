using System;
using System.Threading;
using Foundatio.Messaging;
using Microsoft.AspNetCore.Components;

namespace Cinder.UI.Infrastructure.Components
{
    public abstract class EventBusComponent : ComponentBase, IDisposable
    {
        protected readonly CancellationTokenSource ComponentLifetimeCancellationTokenSource = new CancellationTokenSource();

        [Inject]
        public IMessageBus Bus { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ComponentLifetimeCancellationTokenSource?.Dispose();
            }
        }
    }
}
