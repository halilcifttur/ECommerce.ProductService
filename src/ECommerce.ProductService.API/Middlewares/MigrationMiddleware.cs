using ECommerce.ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.API.Middlewares;

public class MigrationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const int MaxRetries = 3;

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        int attempt = 0;
        while (attempt < MaxRetries)
        {
            try
            {
                await dbContext.Database.MigrateAsync();
                break;
            }
            catch (Exception ex)
            {
                attempt++;

                if (attempt >= MaxRetries)
                {
                    throw;
                }

                await Task.Delay(3000);
            }
        }

        await _next(context);
    }
}