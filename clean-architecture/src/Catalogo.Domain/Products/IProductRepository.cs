using System;

namespace Catalogo.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByCode(string code,CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Product product);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
