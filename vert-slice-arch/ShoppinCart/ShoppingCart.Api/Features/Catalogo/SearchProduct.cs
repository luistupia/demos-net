using ShoppingCart.Api.Shared.Networking.CatalogoApi;

namespace ShoppingCart.Api.Features.Catalogo;

public class SearchProduct
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalogo/{code}", async (string code, 
            ILoggerFactory loggerFactory,
            ICatalogoApiCliente catalogoApiCliente,
            CancellationToken cancellationToken) =>
        {
            loggerFactory.CreateLogger("Endpoint-Catalogo-Code").LogInformation("Buscar producto por el codigo");

            var result = await catalogoApiCliente.GetProductByCodeAsync(code, cancellationToken);
            
            return Results.Ok(result);
        });
    }
}
