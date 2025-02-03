using MediatR;

namespace ECommerce.ProductService.Application.Features.Products.Commands;

public record AddProductCommand(string Name, int Stock) : IRequest;
