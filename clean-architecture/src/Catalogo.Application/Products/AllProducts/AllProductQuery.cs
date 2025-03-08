using Catalogo.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.Products.AllProducts;

public sealed class AllProductQuery : IRequest<List<ProductDto>>
{
    public HttpContext? HttpContext { get; set; }
}