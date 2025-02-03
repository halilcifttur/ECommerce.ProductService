namespace ECommerce.ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Stock { get; set; }
}
