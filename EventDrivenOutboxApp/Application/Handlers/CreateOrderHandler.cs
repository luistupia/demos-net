using System.Text.Json;
using Domain;
using Domain.Events;
using Infrastructure.Repositories;
using Outbox;

namespace Application.Handlers;

public class CreateOrderHandler(IUnitOfWork uow)
{
    public async Task<Guid> HandleAsync(CreateOrderCommand command)
    {
        var order = new Order
        {
            ProductName = command.ProductName,
            Quantity = command.Quantity,
            Price = command.Price
        };

        var eventObj = new OrderPlacedEvent
        {
            OrderId = order.Id,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Price = order.Price
        };

        var outbox = new OutboxMessage
        {
            Type = nameof(OrderPlacedEvent),
            Payload = JsonSerializer.Serialize(eventObj)
        };

        await uow.Repository<Order>().AddAsync(order);
        await uow.Repository<OutboxMessage>().AddAsync(outbox);
        await uow.SaveChangesAsync();

        return order.Id;
    }
}