namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface IHashService
{
    Task<string> GetSecretKey();
    Task<string> HashPassword(string password);
}
