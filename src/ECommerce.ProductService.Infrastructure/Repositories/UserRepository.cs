using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Domain.Entities;
using ECommerce.ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext applicationDbContext) : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task AddAsync(User user)
    {
        await _applicationDbContext.Users.AddAsync(user);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
