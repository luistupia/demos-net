namespace Domain;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}