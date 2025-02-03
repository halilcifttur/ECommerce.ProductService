using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Domain.Entities;
using MediatR;

namespace ECommerce.ProductService.Application.Features.Products.Commands;

public class AddProductCommandHandler(IProductRepository productRepository) : IRequestHandler<AddProductCommand>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        await _productRepository.AddAsync(new Product { Id = Guid.NewGuid(), Name = request.Name, Stock = request.Stock });
    }
}
