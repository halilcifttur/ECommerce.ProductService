using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Domain.Entities;
using MediatR;

namespace ECommerce.ProductService.Application.Features.Users.Commands;

public class AddUserCommandHandler(IUserRepository userRepository, IHashService hashService) : IRequestHandler<AddUserCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHashService _hashService = hashService;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = await _hashService.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.AddAsync(user);
    }
}