namespace ShoppingCart.Api.Shared.Domain.Entities;

public class Cart (Guid id,
    string code,
    string userId) : Entity(id)
{
    private Cart() : this(Guid.Empty, string.Empty, string.Empty){}
    
    public string Code { get; private set; } = code;
    public string UserId { get; private set; } = userId;

    public ICollection<Item> Items { get; set; } = [];

    public static Cart Create(string code, string userId)
    {
        return new Cart(Guid.NewGuid(), code, userId);
    }
}