namespace webapi.Core.Application.Events;

public interface IEvent { }

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent @event);
}

public interface IEventNotifier
{
    Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
}