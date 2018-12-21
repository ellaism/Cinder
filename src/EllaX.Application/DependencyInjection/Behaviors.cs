using Autofac;
using EllaX.Application.Behaviors;
using MediatR;
using MediatR.Pipeline;

// ReSharper disable once CheckNamespace
namespace EllaX.DependencyInjection
{
    public static class Behaviors
    {
        public static void RegisterBehaviors(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPerformanceBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
