namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface IRedisTokenService
{
    Task StoreTokenAsync(Guid userId, string token, TimeSpan? expiration = null);
    Task<string?> RetrieveTokenAsync(Guid userId);
    Task RemoveTokenAsync(Guid userId);
}
