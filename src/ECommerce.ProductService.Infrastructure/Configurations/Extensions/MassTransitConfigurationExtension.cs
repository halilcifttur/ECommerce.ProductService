using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.ProductService.Infrastructure.Configurations.Models;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class MassTransitConfigurationExtension
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

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
