using MediatR;
using ECommerce.ProductService.Application.Features.Auths.Behaviors.Interfaces;
using ECommerce.ProductService.Application.Features.Auths.Dtos;

namespace ECommerce.ProductService.Application.Features.Auths.Commands;

public record LoginCommand(string Username, string Password) : IRequest<TokenDto>, IRequireUserCredentialsValidation;