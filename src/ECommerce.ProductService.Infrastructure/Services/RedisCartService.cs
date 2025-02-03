using ECommerce.ProductService.Application.Features.Carts.Dtos;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ECommerce.ProductService.Infrastructure.Services;

public class RedisCartService(IConnectionMultiplexer connectionMultiplexer, ICurrentUserService currentUserService) : IRedisCartService
{
    private readonly IDatabase _redisDatabase = connectionMultiplexer.GetDatabase();
    private readonly ICurrentUserService _currentUserService = currentUserService;

    private string CartKey => $"cart:{_currentUserService.UserId}";

    public async Task<List<CartItemDto>> GetCartAsync()
    {
        var cartJson = await _redisDatabase.StringGetAsync(CartKey);
        if (cartJson.IsNullOrEmpty)
            return [];

        var cart = JsonSerializer.Deserialize<List<CartItemDto>>(cartJson.ToString()) ?? [];

        var validCartItems = new List<CartItemDto>();
        foreach (var item in cart)
        {
            if (await _redisDatabase.KeyExistsAsync($"product:{item.ProductId}"))
            {
                validCartItems.Add(item);
            }
        }

        if (validCartItems.Count != cart.Count)
        {
            await _redisDatabase.StringSetAsync(CartKey, JsonSerializer.Serialize(validCartItems), TimeSpan.FromHours(1));
        }

        return validCartItems;
    }

    public async Task<bool> AddToCartAsync(CartItemDto cartItem)
    {
        var cart = await GetCartAsync();
        var existingItem = cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId);

        if (existingItem != null)
        {
            cart.Remove(existingItem);
            cart.Add(existingItem with { Quantity = existingItem.Quantity + cartItem.Quantity });
        }
        else
        {
            cart.Add(cartItem);
        }

        await _redisDatabase.StringSetAsync(CartKey, JsonSerializer.Serialize(cart), TimeSpan.FromHours(1));
        return true;
    }
}
