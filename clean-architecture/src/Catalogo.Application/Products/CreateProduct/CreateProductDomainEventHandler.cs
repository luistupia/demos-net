using Catalogo.Domain.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalogo.Application.Products.CreateProduct;

public sealed class CreateProductDomainEventHandler(ILoggerFactory loggerFactory)
    : INotificationHandler<ProductCreatedDomainEvent>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<CreateProductDomainEventHandler>();

    public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Se creado un objeto producto {notification.ProductId}");
        return Task.CompletedTask;
    }
}