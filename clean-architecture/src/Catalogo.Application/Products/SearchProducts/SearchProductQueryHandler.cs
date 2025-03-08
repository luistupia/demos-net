using Catalogo.Application.Dtos;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Products.SearchProducts;

internal sealed class SearchProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<SearchProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(SearchProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByCode(request.Code!,cancellationToken);
        return product!.ToDto(request.HttpContext!);
    }
}