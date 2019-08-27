using Cinder.Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nethereum.Parity;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class Blockchain
    {
        public static void AddBlockchain(this IServiceCollection services)
        {
            services.AddSingleton<IWeb3Parity>(sp =>
            {
                IOptions<Settings> options = sp.GetService<IOptions<Settings>>();

                return new Web3Parity(options.Value.Node.RpcUrl);
            });
        }
    }
}
