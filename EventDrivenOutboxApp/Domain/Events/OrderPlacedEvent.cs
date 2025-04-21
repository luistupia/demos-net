namespace Domain.Events;

public class OrderPlacedEvent
{
    public Guid OrderId { get; set; }
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}