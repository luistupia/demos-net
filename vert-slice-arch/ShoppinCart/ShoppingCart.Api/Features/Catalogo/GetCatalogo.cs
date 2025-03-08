using ShoppingCart.Api.Shared.Networking.CatalogoApi;

namespace ShoppingCart.Api.Features.Catalogo;

public static class GetCatalogo
{
    public static void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalogo", async (ILoggerFactory loggerFactory,
            ICatalogoApiCliente catalogoApiCliente,
            CancellationToken cancellationToken) =>
        {
            loggerFactory.CreateLogger("EndpointCatalogo-Get").LogInformation("Catalogo de productos");
            var result = await catalogoApiCliente.GetProductsAsync(cancellationToken);
            return Results.Ok(result);
        });
    }
}