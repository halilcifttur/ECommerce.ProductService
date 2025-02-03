using ECommerce.ProductService.Messaging.Infrastructure.Configurations.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Messaging.Infrastructure.Configurations.Extensions;

public static class SettingConfigurationExtension
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));

        return services;
    }
}