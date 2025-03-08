using ShoppingCart.Api.Shared.Domain.Models;

namespace ShoppingCart.Api.Shared.Networking.CatalogoApi;

public sealed class CatalogoApiService(HttpClient httpClient)
{
    public async Task<IEnumerable<Catalogo>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var content = await httpClient.GetFromJsonAsync<IEnumerable<Catalogo>>("/api/products",cancellationToken);
        return content!;
    }
}