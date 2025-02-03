namespace ECommerce.ProductService.Messaging.Domain.Entities;

public class MessageInbox
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
}