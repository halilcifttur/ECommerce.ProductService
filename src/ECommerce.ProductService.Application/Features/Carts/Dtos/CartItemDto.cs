namespace ECommerce.ProductService.Application.Features.Carts.Dtos;

public record CartItemDto(Guid ProductId, string ProductName, int Quantity);