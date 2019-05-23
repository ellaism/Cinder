using System;
using System.IO;
using System.Threading.Tasks;
using EllaX.Api.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EllaX.IntegrationTests
{
    public class SliceFixture
    {
        private static readonly IConfigurationRoot Configuration;
        private static readonly IServiceScopeFactory ScopeFactory;

        static SliceFixture()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Startup startup = new Startup(Configuration);
            ServiceCollection services = new ServiceCollection();
            startup.ConfigureServices(services);
            ServiceProvider provider = services.BuildServiceProvider();
            ScopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (IServiceScope scope = ScopeFactory.CreateScope())
            {
                await action(scope.ServiceProvider).ConfigureAwait(false);
            }
        }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (IServiceScope scope = ScopeFactory.CreateScope())
            {
                T result = await action(scope.ServiceProvider).ConfigureAwait(false);

                return result;
            }
        }

        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}
