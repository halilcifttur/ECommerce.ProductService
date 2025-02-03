using ECommerce.ProductService.Application.Features.Auths.Behaviors;
using ECommerce.ProductService.Application.Features.Auths.Contexts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ProductService.Application.Infrastructure.Configurations.Extensions;

public static class PipelineBehaviorConfigurationExtension
{
    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
    {
        services.AddScoped<UserCredentialsRequestContext>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateUserCredentialsBehavior<,>));

        return services;
    }
}
