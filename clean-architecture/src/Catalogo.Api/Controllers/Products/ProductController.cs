using Catalogo.Application.Products.AllProducts;
using Catalogo.Application.Products.CreateProduct;
using Catalogo.Application.Products.SearchProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Api.Controllers.Products;

[ApiController]
[Route("api/products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet("code/{value}")]
    public async Task<IActionResult> GetByCode(string value)
    {
        var context = HttpContext;
        var query = new SearchProductQuery { Code = value,HttpContext = context};
        var product = await sender.Send(query);
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var context = HttpContext;
        return Ok(await sender.Send(new AllProductQuery {HttpContext = context}));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
    {
        var product = await sender.Send(new CreateProductCommand(request.Nombre,request.Descripcion,request.Precio,request.CategoryId));
        return Ok(product);
    }
}