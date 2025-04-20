using ShoppingCart.Api.Shared.Domain.Models;

namespace ShoppingCart.Api.Shared.Networking.CatalogoApi;

public sealed class CatalogoApiService(HttpClient httpClient)
{
    public async Task<IEnumerable<Catalogo>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var content = await httpClient.GetFromJsonAsync<IEnumerable<Catalogo>>("/api/products",cancellationToken);
        return content!;
    }
    
    public async Task<Catalogo?> GetProductByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var content = await httpClient.GetFromJsonAsync<Catalogo>(
            $"api/products/code/{code}", 
            cancellationToken);
        return content;
    }
}