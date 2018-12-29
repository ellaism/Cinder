using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class AuthorizationPolicies
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options => { options.AddPolicy("Management", policy => policy.RequireClaim("Manager")); });
        }
    }
}
