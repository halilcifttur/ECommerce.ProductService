using ECommerce.ProductService.Application.Features.Users.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Application.Infrastructure.Configurations.Extensions;

public static class MediatRConfigurationExtension
{
    public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(AddUserCommand).Assembly);
        });

        return services;
    }
}