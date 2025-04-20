namespace ShoppingCart.Api.Shared.Domain.Entities;

public abstract class Entity(Guid id)
{
    protected Entity() : this(Guid.Empty){}
    public Guid Id { get; set; } = id;
    public DateTime CreateOn { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public DateTime LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
}