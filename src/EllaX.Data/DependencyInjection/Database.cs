using System.Reflection;
using EllaX.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class Database
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string migrationsAssembly = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); }));
        }
    }
}
