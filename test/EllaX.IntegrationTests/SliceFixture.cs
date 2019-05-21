using System.IO;
using EllaX.Api.Infrastructure;
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

        //public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        //{
        //    // todo
        //}

        //public static Task SendAsync(IRequest request)
        //{
        //    // todo
        //}
    }
}
