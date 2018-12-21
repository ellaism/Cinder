using System;
using EllaX.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Extensions
{
    public static class HostingExtensions
    {
        public static void ApplyDbMigrations(this IWebHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                try
                {
                    ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    ApplicationInitializer.Initialize(context);
                }
                catch (Exception e)
                {
                    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occurred while migrating or initializing the database");
                }
            }
        }
    }
}
