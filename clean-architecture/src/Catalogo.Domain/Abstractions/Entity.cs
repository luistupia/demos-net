namespace Catalogo.Domain.Abstractions;
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public Guid Id { get; init;}
    
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {}
     

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}