namespace ECommerce.ProductService.Application.Features.Auths.Behaviors.Interfaces;

public interface IRequireUserCredentialsValidation
{
    public string Username { get; }
    public string Password { get; }
}
