namespace ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Username { get; }
}