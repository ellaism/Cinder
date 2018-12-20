using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMediation(this IServiceCollection services)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
