using ECommerce.ProductService.Domain.Entities;

namespace ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetListAsync();
    Task<Product?> GetByIdAsync(Guid productId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
