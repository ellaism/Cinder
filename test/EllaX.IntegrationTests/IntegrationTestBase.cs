using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Raven.Client.Documents;
using Raven.TestDriver;
using Xunit;

namespace EllaX.IntegrationTests
{
    public abstract class IntegrationTestBase : RavenTestDriver, IAsyncLifetime
    {
        private static readonly AsyncLock Mutex = new AsyncLock();

        private static bool _initialized;

        public virtual async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            using (await Mutex.LockAsync())
            {
                if (_initialized)
                {
                    return;
                }

                ConfigureServer(new TestServerOptions {DataDirectory = "C:\\RavenDBTestDir"});

                _initialized = true;
            }
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        // This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }
    }
}
