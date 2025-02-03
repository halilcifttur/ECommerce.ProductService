using ECommerce.ProductService.Messaging.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.ProductService.Messaging.Middlewares;

public static class MigrationMiddleware
{
    private const int MaxRetries = 3;
    private const int DelayBetweenRetriesMs = 3000;

    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<MessagingDbContext>();

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

                await Task.Delay(DelayBetweenRetriesMs);
            }
        }
    }
}