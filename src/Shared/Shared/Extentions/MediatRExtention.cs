using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviour;
using System.Reflection;

namespace Shared.Extentions
{
    public static class MediatRExtention
    {
        public static IServiceCollection AddMediatRWithAssemblies(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }
    }
}
