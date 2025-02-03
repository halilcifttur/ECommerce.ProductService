using ECommerce.ProductService.API.Infrastructure.Services;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

namespace ECommerce.ProductService.API.Infrastructure.Configurations.Extensions;

public static class ApiServiceConfigurationExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
