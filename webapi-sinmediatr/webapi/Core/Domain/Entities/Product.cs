namespace webapi.Core.Domain.Entities;

public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
}