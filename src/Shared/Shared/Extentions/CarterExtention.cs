using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extentions
{
    public static class CarterExtention
    {
        public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddCarter(configurator: config =>
            {
                foreach (var assembly in assemblies)
                {
                    var carterModules = assembly.GetTypes()
                                        .Where(t => typeof(ICarterModule).IsAssignableFrom(t)).ToArray();
                    config.WithModules(carterModules);
                }

            });
            return services;
        }
    }
}
