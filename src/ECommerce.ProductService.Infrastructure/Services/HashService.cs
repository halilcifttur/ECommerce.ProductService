using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Infrastructure.Configurations.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.ProductService.Infrastructure.Services;

public class HashService(IOptions<HashSettings> hashSettings) : IHashService
{
    private readonly HashSettings _hashSettings = hashSettings.Value;

    public Task<string> GetSecretKey()
    {
        return Task.FromResult(_hashSettings.SecretKey);
    }

    public Task<string> HashPassword(string password)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hashSettings.SecretKey));
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = hmac.ComputeHash(passwordBytes);
        return Task.FromResult(Convert.ToBase64String(hash));
    }
}
