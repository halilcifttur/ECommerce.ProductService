using MediatR;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

namespace ECommerce.ProductService.Application.Features.Auths.Commands;

public class LogoutCommandHandler(IJwtTokenService jwtTokenService, ICurrentUserService currentUserService) : IRequestHandler<LogoutCommand>
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _jwtTokenService.InvalidateTokenAsync(_currentUserService.UserId);
    }
}
