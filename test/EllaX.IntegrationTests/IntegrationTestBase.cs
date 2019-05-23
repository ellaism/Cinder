﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace EllaX.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
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

                // todo

                _initialized = true;
            }
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}