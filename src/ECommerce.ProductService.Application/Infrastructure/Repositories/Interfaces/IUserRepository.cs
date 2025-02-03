using ECommerce.ProductService.Domain.Entities;

namespace ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
}
