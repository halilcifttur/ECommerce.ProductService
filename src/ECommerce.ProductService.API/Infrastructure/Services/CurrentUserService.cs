using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using System.Security.Claims;

namespace ECommerce.ProductService.API.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

    public string Username => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}