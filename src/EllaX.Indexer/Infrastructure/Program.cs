using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EllaX.Configuration.Helpers;
using EllaX.Core.Exceptions;
using EllaX.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EllaX.Indexer.Infrastructure
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = ConfigurationHelpers.GetConfiguration();

        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            try
            {
                Log.Information("Starting EllaX Indexer v{Version}", Constants.Version);
                IHost host = BuildHost(args);
                await host.RunAsync();

                return 0;
            }
            catch (Exception e)
            {
                if (!(e is LoggedException))
                {
                    Log.Fatal(e, "{Class} -> {Method} -> EllaX Indexer terminated unexpectedly", nameof(Program), nameof(Main));
                }

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHost BuildHost(string[] args)
        {
            return new HostBuilder().ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddConfiguration(Configuration);
                    configApp.AddCommandLine(args);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    // ellax
                    services.AddDatabase(Configuration);
                    services.AddMapper();
                    services.AddMediation();
                    services.AddIndexingHosting();
                })
                .ConfigureContainer<ContainerBuilder>((context, builder) =>
                {
                    builder.RegisterClients();
                    builder.RegisterIndexing();
                })
                .UseSerilog()
                .UseConsoleLifetime()
                .Build();
        }
    }
}
