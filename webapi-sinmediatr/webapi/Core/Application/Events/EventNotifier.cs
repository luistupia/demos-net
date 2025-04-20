namespace webapi.Core.Application.Events;

public class EventNotifier(IServiceProvider provider) : IEventNotifier
{
    public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var handlers = provider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers)
            await handler.Handle(@event);
    }
}