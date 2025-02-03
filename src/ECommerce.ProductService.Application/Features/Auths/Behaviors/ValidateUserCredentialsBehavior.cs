using MediatR;
using ECommerce.ProductService.Application.Features.Auths.Behaviors.Interfaces;
using ECommerce.ProductService.Application.Features.Auths.Contexts;
using System.Security.Cryptography;
using System.Text;
using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;

namespace ECommerce.ProductService.Application.Features.Auths.Behaviors;

public class ValidateUserCredentialsBehavior<TRequest, TResponse>(UserCredentialsRequestContext userCredentialsRequestContext, IUserRepository userRepository, IHashService hashService) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly UserCredentialsRequestContext _userCredentialsRequestContext = userCredentialsRequestContext;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHashService _hashService = hashService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IRequireUserCredentialsValidation validationRequest)
        {
            var user = await _userRepository.GetByUsernameAsync(validationRequest.Username);
            if (user == null || !await VerifyPasswordHash(validationRequest.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            _userCredentialsRequestContext.ValidatedUser = user;
        }

        return await next();
    }

    private async Task<bool> VerifyPasswordHash(string password, string storedHash)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(await _hashService.GetSecretKey()));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return storedHash == computedHash;
    }
}
