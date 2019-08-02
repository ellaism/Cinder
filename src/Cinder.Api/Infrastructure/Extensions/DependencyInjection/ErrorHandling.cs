using Cinder.Api.Infrastructure.Middleware;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.DependencyInjection
{
    public static class ErrorHandling
    {
        public static void AddErrorHandling(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlingMiddleware>();
        }
    }
}
