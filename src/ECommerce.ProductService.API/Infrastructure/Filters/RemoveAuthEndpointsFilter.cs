using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ECommerce.ProductService.API.Infrastructure.Filters;

public class RemoveAuthEndpointsFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var pathsToRemove = swaggerDoc.Paths
            .Where(path => path.Key.StartsWith("/api/Auth", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var path in pathsToRemove)
        {
            swaggerDoc.Paths.Remove(path.Key);
        }
    }
}