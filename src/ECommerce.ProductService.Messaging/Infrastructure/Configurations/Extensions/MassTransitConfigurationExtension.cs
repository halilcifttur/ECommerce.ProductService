using ECommerce.ProductService.Messaging.Events.Consumers;
using ECommerce.ProductService.Messaging.Infrastructure.Configurations.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce.ProductService.Messaging.Infrastructure.Configurations.Extensions;

public static class MassTransitConfigurationExtension
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<UpdateProductStockEventConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqSettings = context.GetService<IOptions<RabbitMqSettings>>()?.Value ?? throw new InvalidOperationException("RabbitMQ settings are not configured.");

                configurator.Host(new Uri(rabbitMqSettings.Host), h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}