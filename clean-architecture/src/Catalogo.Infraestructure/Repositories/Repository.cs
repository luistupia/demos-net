using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infraestructure.Repositories;

internal abstract class Repository<T>(CatalogoDbContext dbContext)
    where T : Entity
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public void Add(T entity) => dbContext.Add(entity);
}