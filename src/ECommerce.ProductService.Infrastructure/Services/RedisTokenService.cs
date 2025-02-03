using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using StackExchange.Redis;

namespace ECommerce.ProductService.Infrastructure.Services;

public class RedisTokenService(IConnectionMultiplexer connectionMultiplexer) : IRedisTokenService
{
    private readonly IDatabase _redisDatabase = connectionMultiplexer.GetDatabase();

    public async Task StoreTokenAsync(Guid userId, string token, TimeSpan? expiration = null)
    {
        var key = $"jwt:{userId}";
        await _redisDatabase.StringSetAsync(key, token, expiration);
    }

    public async Task<string?> RetrieveTokenAsync(Guid userId)
    {
        var key = $"jwt:{userId}";
        var cachedToken = await _redisDatabase.StringGetAsync(key);
        return cachedToken.HasValue ? cachedToken.ToString() : null;
    }

    public async Task RemoveTokenAsync(Guid userId)
    {
        var key = $"jwt:{userId}";
        await _redisDatabase.KeyDeleteAsync(key);
    }
}