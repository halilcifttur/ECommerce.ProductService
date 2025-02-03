using ECommerce.ProductService.Application.Features.Carts.Dtos;
using MediatR;

namespace ECommerce.ProductService.Application.Features.Carts.Commands;

public record AddToCartCommand(Guid ProductId, int Quantity) : IRequest<AddToCartResponseDto>;
