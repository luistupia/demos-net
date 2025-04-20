using webapi.Core.Application.Events;

namespace webapi.Core.Application.Products.Events;

public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : IEventHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent @event)
    {
        logger.LogInformation("Producto creado: {@Product}", @event.Entity);
        return Task.CompletedTask;
    }
}