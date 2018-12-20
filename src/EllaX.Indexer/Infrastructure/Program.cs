using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Indexer.Infrastructure
{
    public class Program
    {
        public static async Task Main(string[] args)

        {
            IHost host = new HostBuilder().ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("hostsettings.json", true);
                configHost.AddEnvironmentVariables("PREFIX_");
                configHost.AddCommandLine(args);
            }).ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.AddJsonFile("appsettings.json", true);
                configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                configApp.AddEnvironmentVariables("PREFIX_");
                configApp.AddCommandLine(args);
            }).ConfigureServices((hostContext, services) =>
            {
                //services.AddHostedService<LifetimeEventsHostedService>();
            }).ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConsole();
                configLogging.AddDebug();
            }).UseConsoleLifetime().Build();

            await host.RunAsync();
        }
    }
}
