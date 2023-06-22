using EventBus.Base.Abstraction;
using EventBus.UnitTests.Events.Events;

namespace EventBus.UnitTests.Events.EventHandlers;

public class ContactCreatedIntegrationEventHandler : IIntegrationEventHandler<ContactCreatedIntegrationEvent>
{
    public Task Handle(ContactCreatedIntegrationEvent @event)
    {
        Console.WriteLine("Handle method worked with id: " + @event.Id);
        return Task.CompletedTask;
    }
}