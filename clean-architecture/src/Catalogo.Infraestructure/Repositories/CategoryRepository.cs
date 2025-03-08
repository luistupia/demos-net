using Catalogo.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infraestructure.Repositories;

internal sealed class CategoryRepository(CatalogoDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
    private readonly CatalogoDbContext _dbContext = dbContext;

    public async Task<List<Category>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>().ToListAsync(cancellationToken);
    }
}