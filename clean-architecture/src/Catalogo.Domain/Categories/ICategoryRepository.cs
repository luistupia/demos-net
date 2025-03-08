using System;

namespace Catalogo.Domain.Categories;

public interface ICategoryRepository
{
    Task<List<Category>?> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Category category);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
