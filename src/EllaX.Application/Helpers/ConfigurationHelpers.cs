using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EllaX.Application.Helpers
{
    public static class ConfigurationHelpers
    {
        public static IConfigurationRoot GetConfiguration()
        {
            return BuildConfiguration(Directory.GetCurrentDirectory());
        }

        public static IConfigurationRoot GetConfiguration(IHostingEnvironment env)
        {
            return BuildConfiguration(env.ContentRootPath);
        }

        private static IConfigurationRoot BuildConfiguration(string basePath)
        {
            return new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
