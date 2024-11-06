namespace RikaWebApp.Models;

public class CartItem
{
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
