using ECommerce.ProductService.Application.Infrastructure.Services.Interfaces;
using System.Security.Claims;

namespace ECommerce.ProductService.API.Middlewares;

public class TokenValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (userId != null && token != null)
        {
            using var scope = serviceProvider.CreateScope();
            var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

            var isValid = await jwtTokenService.ValidateTokenAsync(Guid.Parse(userId), token);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }
        }

        await _next(context);
    }
}