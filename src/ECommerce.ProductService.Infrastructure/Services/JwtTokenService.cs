using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using ECommerce.ProductService.Infrastructure.Configurations.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.ProductService.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtSettings> jwtSettings, IRedisTokenService redisTokenService) : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly IRedisTokenService _redisTokenService = redisTokenService;

    public async Task<string> GenerateToken(Guid userId, string username)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        await _redisTokenService.StoreTokenAsync(userId, jwt, TimeSpan.FromMinutes(_jwtSettings.ExpiryInMinutes));
        return jwt;
    }

    public async Task<bool> ValidateTokenAsync(Guid userId, string token)
    {
        var cachedToken = await _redisTokenService.RetrieveTokenAsync(userId);
        return cachedToken == token;
    }

    public async Task InvalidateTokenAsync(Guid userId)
    {
        await _redisTokenService.RemoveTokenAsync(userId);
    }
}
