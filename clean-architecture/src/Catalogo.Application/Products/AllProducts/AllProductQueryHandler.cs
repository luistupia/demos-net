using Catalogo.Application.Dtos;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Products.AllProducts;

public sealed class AllProductQueryHandler(IProductRepository productRepository) : IRequestHandler<AllProductQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(AllProductQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.ConvertAll(x => x.ToDto(request.HttpContext!));
    }
}