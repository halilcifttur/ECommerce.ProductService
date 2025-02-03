using ECommerce.ProductService.Messaging.Infrastructure.Configurations.Models;
using ECommerce.ProductService.Messaging.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.ProductService.Messaging.Infrastructure.Configurations.Extensions;

public static class DbContextConfigurationExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<MessagingDbContext>((serviceProvider, options) =>
        {
            var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseNpgsql(databaseSettings.DefaultConnection);
        });

        return services;
    }
}