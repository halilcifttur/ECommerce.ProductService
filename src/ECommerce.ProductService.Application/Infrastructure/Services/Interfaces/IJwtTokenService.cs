namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken(Guid userId, string username);
    Task<bool> ValidateTokenAsync(Guid userId, string token);
    Task InvalidateTokenAsync(Guid userId);
}