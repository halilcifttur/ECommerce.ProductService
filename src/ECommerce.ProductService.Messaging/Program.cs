using ECommerce.ProductService.Messaging.Infrastructure.Configurations.Extensions;
using ECommerce.ProductService.Messaging.Middlewares;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSettings(context.Configuration);
        services.AddDbContext();
        services.AddMassTransitWithRabbitMQ();
    })
    .Build();

await host.ApplyMigrations();

await host.RunAsync();