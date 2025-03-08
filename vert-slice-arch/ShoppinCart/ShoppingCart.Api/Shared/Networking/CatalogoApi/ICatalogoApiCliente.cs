using System;
using ShoppingCart.Api.Shared.Domain.Models;

namespace ShoppingCart.Api.Shared.Networking.CatalogoApi;

public interface ICatalogoApiCliente
{
    Task<IEnumerable<Catalogo>> GetProductsAsync(CancellationToken cancellationToken);
    Task<Catalogo> GetProductByCodeAsync(string code,CancellationToken cancellationToken);
}
