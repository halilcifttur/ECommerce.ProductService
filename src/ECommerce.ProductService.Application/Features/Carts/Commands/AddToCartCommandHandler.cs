using ECommerce.ProductService.Application.Features.Carts.Dtos;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Shared.Events.Models;
using MediatR;

namespace ECommerce.ProductService.Application.Features.Carts.Commands;

public class AddToCartCommandHandler(IRedisProductService redisProductService, IRedisCartService redisCartService, IEventBusService eventBusService) : IRequestHandler<AddToCartCommand, AddToCartResponseDto>
{
    private readonly IRedisProductService _redisProductService = redisProductService;
    private readonly IRedisCartService _redisCartService = redisCartService;
    private readonly IEventBusService _eventBusService = eventBusService;

    public async Task<AddToCartResponseDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _redisProductService.GetProductAsync(request.ProductId) ?? throw new InvalidOperationException("Product not found!");
        var stockReserved = await _redisProductService.TryReserveStockAsync(product.Id, request.Quantity);
        if (!stockReserved)
        {
            throw new InvalidOperationException("Insufficient stock!");
        }

        var isAddedToCart = await _redisCartService.AddToCartAsync(new CartItemDto(product.Id, product.Name, request.Quantity));
        if (!isAddedToCart)
        {
            await _redisProductService.RestoreStockAsync(request.ProductId, request.Quantity);
            throw new InvalidOperationException("Cart couldn't be updated!");
        }

        var cart = await _redisCartService.GetCartAsync();

        await _eventBusService.PublishAsync(new UpdatedProductStockEvent(Guid.NewGuid(), request.ProductId, -request.Quantity), cancellationToken);

        return new AddToCartResponseDto(cart);
    }
}
