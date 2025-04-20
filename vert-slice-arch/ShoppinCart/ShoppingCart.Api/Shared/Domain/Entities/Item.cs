namespace ShoppingCart.Api.Shared.Domain.Entities;

public class Item(Guid id,
    string code,
    string imageUrl,
    int qty,
    decimal price,
    string name,
    string description,
    Guid cartId
    ) : Entity(id)
{
    private Item() : this(Guid.Empty,
        string.Empty, 
        string.Empty, 
        0,0.0m,
        string.Empty,
        string.Empty,
        Guid.Empty){}
    
    public string Code { get; private set; } = code;
    public string ImageUrl { get; private set; }  = imageUrl;
    public decimal Price { get; private set; } = price;
    public int Quantity { get; private set; } = qty;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;

    public Guid CartId { get; private set; } = cartId;
    
    public Cart? Cart { get; private set; }

    public static Item Create(string code,
        string imageUrl,
        int qty,
        decimal price,
        string name,
        string description,
        Guid cartId)
    {
        return new Item(Guid.NewGuid(),code,imageUrl,qty,price,name,description,cartId);
    }
}