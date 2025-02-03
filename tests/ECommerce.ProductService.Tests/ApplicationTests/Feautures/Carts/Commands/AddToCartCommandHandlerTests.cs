using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Application.Features.Carts.Commands;
using ECommerce.ProductService.Application.Features.Products.Dtos;
using ECommerce.ProductService.Application.Features.Carts.Dtos;
using ECommerce.ProductService.Shared.Events.Models;
using FluentAssertions;
using Moq;

namespace ECommerce.ProductService.Tests.ApplicationTests.Feautures.Carts.Commands;

public class AddToCartCommandHandlerTests
{
    private readonly Mock<IRedisProductService> _mockRedisProductService;
    private readonly Mock<IRedisCartService> _mockRedisCartService;
    private readonly Mock<IEventBusService> _mockEventBusService;
    private readonly AddToCartCommandHandler _handler;

    public AddToCartCommandHandlerTests()
    {
        _mockRedisProductService = new Mock<IRedisProductService>();
        _mockRedisCartService = new Mock<IRedisCartService>();
        _mockEventBusService = new Mock<IEventBusService>();

        _handler = new AddToCartCommandHandler(
            _mockRedisProductService.Object,
            _mockRedisCartService.Object,
            _mockEventBusService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReserveStockAndAddToCartSuccessfully()
    {
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "Test Product", 10);
        var request = new AddToCartCommand(productId, 2);

        _mockRedisProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(product);
        _mockRedisProductService.Setup(p => p.TryReserveStockAsync(productId, 2)).ReturnsAsync(true);
        _mockRedisCartService.Setup(c => c.AddToCartAsync(It.IsAny<CartItemDto>())).ReturnsAsync(true);
        _mockRedisCartService.Setup(c => c.GetCartAsync()).ReturnsAsync(new List<CartItemDto>());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        _mockEventBusService.Verify(e => e.PublishAsync(It.IsAny<UpdatedProductStockEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        var request = new AddToCartCommand(productId, 2);

        _mockRedisProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync((ProductDto)null);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Product not found!");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenStockInsufficient()
    {
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "Test Product", 1);
        var request = new AddToCartCommand(productId, 2);

        _mockRedisProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(product);
        _mockRedisProductService.Setup(p => p.TryReserveStockAsync(productId, 2)).ReturnsAsync(false);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Insufficient stock!");
    }

    [Fact]
    public async Task Handle_ShouldRestoreStock_WhenCartUpdateFails()
    {
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "Test Product", 10);
        var request = new AddToCartCommand(productId, 2);

        _mockRedisProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(product);
        _mockRedisProductService.Setup(p => p.TryReserveStockAsync(productId, 2)).ReturnsAsync(true);
        _mockRedisCartService.Setup(c => c.AddToCartAsync(It.IsAny<CartItemDto>())).ReturnsAsync(false);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Cart couldn't be updated!");
        _mockRedisProductService.Verify(p => p.RestoreStockAsync(productId, 2), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldHandleConcurrentAdditionsCorrectly()
    {
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "Test Product", 5);

        _mockRedisProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(product);
        _mockRedisProductService.SetupSequence(p => p.TryReserveStockAsync(productId, 3))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockRedisCartService.Setup(c => c.AddToCartAsync(It.IsAny<CartItemDto>())).ReturnsAsync(true);
        _mockRedisCartService.Setup(c => c.GetCartAsync()).ReturnsAsync(new List<CartItemDto>());

        var task1 = Task.Run(async () =>
        {
            try
            {
                await _handler.Handle(new AddToCartCommand(productId, 3), CancellationToken.None);
                return (Exception)null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        });

        var task2 = Task.Run(async () =>
        {
            try
            {
                await _handler.Handle(new AddToCartCommand(productId, 3), CancellationToken.None);
                return (Exception)null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        });

        var results = await Task.WhenAll(task1, task2);

        results.Should().ContainSingle(e => e is InvalidOperationException && e.Message == "Insufficient stock!");
        results.Should().ContainSingle(e => e == null);
    }
}