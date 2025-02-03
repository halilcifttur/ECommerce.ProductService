using MediatR;

namespace ECommerce.ProductService.Application.Features.Users.Commands;

public record AddUserCommand(string Username, string Email, string Password) : IRequest;