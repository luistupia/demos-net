using Catalogo.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infraestructure.Repositories;

internal sealed class ProductRepository(CatalogoDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
{
    private readonly CatalogoDbContext _dbContext = dbContext;

    public async Task<Product?> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Product>().Where(x=>x.Code == code).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Product>().ToListAsync(cancellationToken);
    }

}