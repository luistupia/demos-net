using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Categories.Events;

namespace Catalogo.Domain.Categories;

public class Category : Entity
{
    public string? Name { get; private set; }
    private Category(Guid id,string name) : base(id)
    {
        Name = name;
    }

    private Category()
    {
    }
    public static Category Create(string name)
    {
        var category = new Category(Guid.NewGuid(), name);
        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));
        return category;
    }
}