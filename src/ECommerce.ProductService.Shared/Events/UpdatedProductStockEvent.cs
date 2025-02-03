namespace ECommerce.ProductService.Shared.Events.Models;

public record UpdatedProductStockEvent(Guid EventId, Guid ProductId, int Quantity);