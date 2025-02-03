using MediatR;
using ECommerce.ProductService.Application.Features.Auths.Contexts;
using ECommerce.ProductService.Application.Features.Auths.Dtos;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

namespace ECommerce.ProductService.Application.Features.Auths.Commands;

public class LoginCommandHandler(UserCredentialsRequestContext userCredentialsRequestContext, IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly UserCredentialsRequestContext _userCredentialsRequestContext = userCredentialsRequestContext;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validatedUser = _userCredentialsRequestContext.ValidatedUser;
        return new TokenDto(Token: await _jwtTokenService.GenerateToken(validatedUser.Id, validatedUser.Username));
    }
}
