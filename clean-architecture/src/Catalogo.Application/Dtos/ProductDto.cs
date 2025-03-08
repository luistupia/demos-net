using Catalogo.Domain.Products;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.Dtos;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product,HttpContext context) =>
        new(
            product.Id,
            product.Name!,
            product.Description!,
            product.Price ?? product.Price!.Value,
            $"{context.Request.Scheme}://{context.Request.Host}/images/{product.ImageUrl}",
            product.Code!,
            product.CategoryId
        );
}


public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    string Code,
    Guid CategoryId
    );