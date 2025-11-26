using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extentions
{
    public static class MassTransitExtentions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq
            (this IServiceCollection services,
            IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.SetInMemorySagaRepositoryProvider();
                x.AddConsumers(assemblies);
                x.AddSagaStateMachines(assemblies);
                x.AddSagas(assemblies);
                x.AddActivities(assemblies);
                //x.UsingInMemory((context, config) =>
                //{
                //    config.ConfigureEndpoints(context);
                //});

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(configuration["MessageBroker:UserName"]!);
                        h.Password(configuration["MessageBroker:Password"]!);
                    });
                    config.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
