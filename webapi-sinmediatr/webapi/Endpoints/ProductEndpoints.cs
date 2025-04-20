using webapi.Core.Application.Mediator;
using webapi.Core.Application.Products;

namespace webapi.Endpoints;

public static class ProductEndpoints
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/", async (int page, int size, IMediator med) =>
            Results.Ok(await med.Send(new GetPagedProductsQuery(page, size))));

        group.MapGet("/{id:int}", async (int id, IMediator med) =>
            Results.Ok(await med.Send(new GetProductByIdQuery(id))));

        group.MapPost("/", async (CreateProductDto dto, IMediator med) =>
        {
            var created = await med.Send(new CreateProductCommand(dto));
            return Results.Created($"/products/{created.Id}", created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateProductDto dto, IMediator med) =>
        {
            if (id != dto.Id) return Results.BadRequest();
            return Results.Ok(await med.Send(new UpdateProductCommand(dto)));
        });

        group.MapDelete("/{id:int}", async (int id, IMediator med) =>
        {
            await med.Send(new DeleteProductCommand(id));
            return Results.NoContent();
        });

        return app;
    }
}