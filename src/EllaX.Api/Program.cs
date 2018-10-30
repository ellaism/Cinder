using System;
using System.IO;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using EllaX.Core.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EllaX.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true).AddEnvironmentVariables().Build();

        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            try
            {
                Log.Information("Starting web host");
                IWebHost host = BuildWebHost(args);
                host.Start();
                await host.WaitForShutdownAsync();

                return 0;
            }
            catch (Exception ex)
            {
                if (!(ex is LoggedException))
                {
                    Log.Fatal(ex, $"{nameof(Program)} -> {nameof(Main)} -> Host terminated unexpectedly");
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
            return WebHost.CreateDefaultBuilder(args).ConfigureServices(services => services.AddAutofac())
                .UseConfiguration(Configuration).UseStartup<Startup>().UseSerilog().Build();
        }
    }
}
