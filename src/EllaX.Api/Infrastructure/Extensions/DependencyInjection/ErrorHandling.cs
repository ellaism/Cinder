using EllaX.Api.Infrastructure.Middleware;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions.DependencyInjection
{
    public static class ErrorHandling
    {
        public static void AddErrorHandling(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlingMiddleware>();
        }
    }
}
