using ECommerce.ProductService.Infrastructure.Configurations.Models;
using ECommerce.ProductService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class DbContextConfigurationExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseNpgsql(databaseSettings.DefaultConnection);
        });

        return services;
    }
}
