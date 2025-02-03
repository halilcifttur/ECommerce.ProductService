using ECommerce.ProductService.Application.Features.Products.Dtos;
using ECommerce.ProductService.Application.Infrastructure.Repositories.Interfaces;
using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using StackExchange.Redis;

namespace ECommerce.ProductService.Infrastructure.Services;

public class RedisProductService(IConnectionMultiplexer connectionMultiplexer, IProductRepository productRepository) : IRedisProductService
{
    private readonly IDatabase _redisDatabase = connectionMultiplexer.GetDatabase();
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ProductDto?> GetProductAsync(Guid productId)
    {
        var key = $"product:{productId}";

        var hashEntries = await _redisDatabase.HashGetAllAsync(key);
        if (hashEntries.Length > 0)
        {
            var productName = hashEntries.FirstOrDefault(x => x.Name == "Name").Value;
            var stock = int.Parse(hashEntries.FirstOrDefault(x => x.Name == "Stock").Value!);
            return new ProductDto(productId, productName!, stock);
        }

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            return null;

        await AddProductAsync(product.Id, product.Name, product.Stock);

        return new ProductDto(product.Id, product.Name, product.Stock);
    }

    public async Task<bool> AddProductAsync(Guid productId, string productName, int quantity)
    {
        var key = $"product:{productId}";

        var exists = await _redisDatabase.KeyExistsAsync(key);
        if (exists)
        {
            return false;
        }

        var transaction = _redisDatabase.CreateTransaction();

        transaction.AddCondition(Condition.KeyNotExists(key));

        var addHashTask = transaction.HashSetAsync(key,
        [
            new HashEntry("Name", productName),
            new HashEntry("Stock", quantity)
        ]);

        var success = await transaction.ExecuteAsync();

        if (success)
            await addHashTask;

        return success;
    }

    public async Task<bool> TryReserveStockAsync(Guid productId, int quantity)
    {
        var key = $"product:{productId}";

        var luaScript = @"
            local stock = tonumber(redis.call('HGET', KEYS[1], 'Stock'))
            if stock == nil or stock < tonumber(ARGV[1]) then
                return -1
            end
            return redis.call('HINCRBY', KEYS[1], 'Stock', -ARGV[1])";

        var stockResult = (int)(await _redisDatabase.ScriptEvaluateAsync(luaScript, new RedisKey[] { key }, new RedisValue[] { quantity }));

        return stockResult >= 0;
    }

    public async Task<bool> RestoreStockAsync(Guid productId, int quantity)
    {
        var luaScript = @"
        local stock = tonumber(redis.call('HGET', KEYS[1], 'Stock'))
        if stock == nil then 
            return -1 
        end
        return redis.call('HINCRBY', KEYS[1], 'Stock', ARGV[1])";

        var stockResult = (int)(await _redisDatabase.ScriptEvaluateAsync(luaScript, new RedisKey[] { $"product:{productId}" }, new RedisValue[] { quantity }));

        return stockResult >= 0;
    }
}
