using ECommerce.ProductService.Messaging.Domain.Entities;
using ECommerce.ProductService.Messaging.Infrastructure.Persistence;
using ECommerce.ProductService.Shared.Events.Models;
using MassTransit;
using System.Text.Json;

namespace ECommerce.ProductService.Messaging.Events.Consumers;

public class UpdateProductStockEventConsumer(MessagingDbContext messagingDbContext) : IConsumer<UpdatedProductStockEvent>
{
    private readonly MessagingDbContext _messagingDbContext = messagingDbContext;

    public async Task Consume(ConsumeContext<UpdatedProductStockEvent> context)
    {
        var messageInbox = new MessageInbox
        {
            Id = Guid.NewGuid(),
            EventType = nameof(UpdatedProductStockEvent),
            Payload = JsonSerializer.Serialize(context.Message),
            IsProcessed = false
        };

        await _messagingDbContext.MessageInboxes.AddAsync(messageInbox);
        await _messagingDbContext.SaveChangesAsync();
    }
}
