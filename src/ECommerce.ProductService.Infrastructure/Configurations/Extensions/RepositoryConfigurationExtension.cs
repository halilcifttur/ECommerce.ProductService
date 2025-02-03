using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Infrastructure.Configurations.Extensions;

public static class RepositoryConfigurationExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
