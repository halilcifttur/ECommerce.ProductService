using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Domain.Entities;
using ECommerce.ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext applicationDbContext) : IProductRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<List<Product>> GetListAsync()
    {
        return await _applicationDbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid productId)
    {
        return await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task AddAsync(Product product)
    {
        await _applicationDbContext.AddAsync(product);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _applicationDbContext.Products.Update(product);
        await _applicationDbContext.SaveChangesAsync();
    }
}
