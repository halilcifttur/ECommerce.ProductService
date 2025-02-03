using ECommerce.ProductService.Infrastructure.Configurations.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class RedisConfigurationExtension
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var redisSettings = provider.GetRequiredService<IOptions<RedisSettings>>().Value;
            return ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        });

        return services;
    }
}
