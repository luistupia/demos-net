using FluentValidation;
using Microsoft.EntityFrameworkCore;
using webapi.Core.Application.Events;
using webapi.Core.Application.Mediator;
using webapi.Core.Domain.Entities;
using webapi.Core.Infrastructure.Persistence;

namespace webapi.Core.Application.Products;

// Comandos y consultas
#region Commands

public record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductDto>;
public record UpdateProductCommand(UpdateProductDto Dto) : IRequest<ProductDto>;
public record DeleteProductCommand(int Id) : IRequest<bool>;

#endregion

#region Queries

public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;
public record GetPagedProductsQuery(int Page, int Size) : IRequest<IEnumerable<ProductDto>>;

#endregion

// Eventos de dominio
#region EventsDomain

public class ProductCreatedEvent(ProductDto e) : IEvent
{
    public ProductDto Entity { get; } = e;
}
public class ProductUpdatedEvent(ProductDto e) : IEvent
{ public ProductDto Entity { get; } = e;
}
public class ProductDeletedEvent(int id) : IEvent
{ public int Id { get; } = id;
}

#endregion

// Handlers
#region Handlers

public class CreateProductHandler(ApiDbContext db, IEventNotifier notifier) : IHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request)
    {
        var prod = new Product { Name = request.Dto.Name, Description = request.Dto.Description };
        db.Products.Add(prod);
        await db.SaveChangesAsync();

        var dto = new ProductDto(prod.Id, prod.Name, prod.Description);
        await notifier.Publish(new ProductCreatedEvent(dto));
        return dto;
    }
}

public class UpdateProductHandler(ApiDbContext db, IEventNotifier notifier) : IHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request)
    {
        var prod = await db.Products.FindAsync(request.Dto.Id)
                   ?? throw new KeyNotFoundException("Product not found");
        prod.Name = request.Dto.Name;
        prod.Description = request.Dto.Description;
        await db.SaveChangesAsync();

        var dto = new ProductDto(prod.Id, prod.Name, prod.Description);
        await notifier.Publish(new ProductUpdatedEvent(dto));
        return dto;
    }
}

public class DeleteProductHandler(ApiDbContext db, IEventNotifier notifier) : IHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request)
    {
        var prod = await db.Products.FindAsync(request.Id)
                   ?? throw new KeyNotFoundException("Product not found");
        db.Products.Remove(prod);
        await db.SaveChangesAsync();

        await notifier.Publish(new ProductDeletedEvent(request.Id));
        return true;
    }
}

public class GetByIdHandler(ApiDbContext db) : IHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request)
    {
        var prod = await db.Products.FindAsync(request.Id)
                   ?? throw new KeyNotFoundException("Product not found");
        return new ProductDto(prod.Id, prod.Name, prod.Description);
    }
}

public class GetPagedHandler(ApiDbContext db) : IHandler<GetPagedProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetPagedProductsQuery request)
        => await db.Products
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description))
            .ToListAsync();
}

#endregion
