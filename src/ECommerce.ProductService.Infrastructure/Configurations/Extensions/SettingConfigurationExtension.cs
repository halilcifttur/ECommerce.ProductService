using ECommerce.ProductService.Infrastructure.Configurations.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class SettingConfigurationExtension
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<HashSettings>(configuration.GetSection("HashSettings"));

        return services;
    }
}