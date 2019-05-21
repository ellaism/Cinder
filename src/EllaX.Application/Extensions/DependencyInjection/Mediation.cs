using System;
using EllaX.Application.Behaviors;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions.DependencyInjection
{
    public static class Mediation
    {
        public static void AddMediation(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
