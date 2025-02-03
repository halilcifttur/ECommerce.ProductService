using ECommerce.ProductService.Domain.Entities;

namespace ECommerce.ProductService.Application.Features.Auths.Contexts;

public class UserCredentialsRequestContext
{
    public required User ValidatedUser { get; set; }
}
