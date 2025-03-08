using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler(IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = Product.Create(request.Name,request.Price,request.Description,null!,null!,request.CategoryId);
        productRepository.Add(newProduct);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newProduct.Id;
    }
}