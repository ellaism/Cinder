using Microsoft.AspNetCore.Builder;
using Rebus.ServiceProvider;

// ReSharper disable once CheckNamespace
namespace Cinder.Extensions.Builder
{
    public static class Messaging
    {
        public static void UseMessaging(this IApplicationBuilder app)
        {
            app.ApplicationServices.UseRebus();
        }
    }
}
