using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using EllaX.Api.Infrastructure.Extensions;
using EllaX.Application.Helpers;
using EllaX.Core.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EllaX.Api.Infrastructure
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = ConfigurationHelpers.GetConfiguration();

        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            try
            {
                Log.Information("Starting EllaX API v{Version}", Constants.Version);
                IWebHost host = BuildWebHost(args);
                // TODO: 20181221 make this optional
                host.ApplyDbMigrations();
                await host.RunAsync();

                return 0;
            }
            catch (Exception e)
            {
                if (!(e is LoggedException))
                {
                    Log.Fatal(e, "{Class} -> {Method} -> EllaX API terminated unexpectedly", nameof(Program), nameof(Main));
                }

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseConfiguration(Configuration)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
        }
    }
}
