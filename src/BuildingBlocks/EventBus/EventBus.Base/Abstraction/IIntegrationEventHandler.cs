using EventBus.Base.Events;

namespace EventBus.Base.Abstraction;

public interface IIntegrationEventHandler<IIntegrationEvent> : IntegrationEventHandler where IIntegrationEvent : IntegrationEvent
{
    Task Handle(IIntegrationEvent @event);
}

public interface IntegrationEventHandler
{
    
}