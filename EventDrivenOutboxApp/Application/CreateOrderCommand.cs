namespace Application;

public record CreateOrderCommand(string ProductName, int Quantity, decimal Price);