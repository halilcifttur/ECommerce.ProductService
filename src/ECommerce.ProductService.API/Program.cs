using ECommerce.ProductService.API.Infrastructure.Configurations.Extensions;
using ECommerce.ProductService.Infrastructure.Configurations.Extensions;
using ECommerce.ProductService.Application.Infrastructure.Configurations.Extensions;
using ECommerce.ProductService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (isDocker)
{
    builder.WebHost.UseUrls("http://*:5000");
}
else
{
    builder.WebHost.UseUrls("http://*:5000", "https://*:5001");
}

builder.Services.AddControllers();
builder.Services.AddSwagger();

// ECommerce.ProductService.Infrastructure
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddDbContext();
builder.Services.AddRedis();
builder.Services.AddRepositories();
builder.Services.AddInfrastructureServices();
builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

// ECommerce.ProductService.Application
builder.Services.AddApplicationMediatR();
builder.Services.AddPipelineBehaviors();

// ECommerce.ProductService.API
builder.Services.AddApiServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<MigrationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
