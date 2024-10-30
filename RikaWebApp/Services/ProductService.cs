namespace RikaWebApp.Services;

public class ProductService
{
    public Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Title = "Nike T-shirt", Price = 100, Ingress = "This is an Ingress", ImageUrl = "/images/Nike-T-shirt.jpg", Category = "Nike Sportswear" },
            new Product { Id = 2, Title = "AIR FORCE 1", Price = 200, Ingress = "This is an Ingress", ImageUrl = "/images/AIR-FORCE-1.jpg", Category = "Nike Sportswear"},
            new Product { Id = 3, Title = "HANS T-shirt", Price = 999, Ingress = "This is an Ingress", ImageUrl = "/images/HANS-T-shirt.jpg", Category = "Hans Sportswear"}
        };
        return Task.FromResult(products);
    }
}

public class Product
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Ingress { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }
}
