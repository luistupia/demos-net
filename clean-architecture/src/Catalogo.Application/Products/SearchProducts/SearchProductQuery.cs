using Catalogo.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.Products.SearchProducts;

public sealed class SearchProductQuery : IRequest<ProductDto>
{
    public string? Code { get; set; }
    public HttpContext? HttpContext { get; set; }
}