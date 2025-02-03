using ECommerce.ProductService.Application.Features.Carts.Dtos;

namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface IRedisCartService
{
    Task<List<CartItemDto>> GetCartAsync();
    Task<bool> AddToCartAsync(CartItemDto cartItem);
}
