using Cinder.Api.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.Builder
{
    public static class ErrorHandling
    {
        public static void UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
