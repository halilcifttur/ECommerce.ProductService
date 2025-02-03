using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using MassTransit;

namespace ECommerce.ProductService.Infrastructure.Services;

public class EventBusService(IPublishEndpoint publishEndpoint) : IEventBusService
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}