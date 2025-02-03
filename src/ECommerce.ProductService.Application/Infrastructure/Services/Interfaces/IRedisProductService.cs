using ECommerce.ProductService.Application.Features.Products.Dtos;

namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface IRedisProductService
{
    Task<ProductDto?> GetProductAsync(Guid productId);
    Task<bool> AddProductAsync(Guid productId, string productName, int quantity);
    Task<bool> TryReserveStockAsync(Guid productId, int quantity);
    Task<bool> RestoreStockAsync(Guid productId, int quantity);
}
