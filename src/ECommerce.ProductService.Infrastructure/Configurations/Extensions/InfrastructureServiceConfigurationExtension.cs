using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class InfrastructureServiceConfigurationExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IEventBusService, EventBusService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRedisTokenService, RedisTokenService>();
        services.AddScoped<IRedisProductService, RedisProductService>();
        services.AddScoped<IRedisCartService, RedisCartService>();

        return services;
    }
}
