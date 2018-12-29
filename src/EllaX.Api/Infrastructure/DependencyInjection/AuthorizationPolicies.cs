using System;
using System.IO;
using System.Reflection;
using EllaX.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class AuthorizationPolicies
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Management", policy => policy.RequireClaim("Manager"));
            });
        }
    }
}
