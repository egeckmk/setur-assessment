using EventBus.Base.Events;

namespace EventBus.UnitTests.Events.Events;

public class ContactCreatedIntegrationEvent : IntegrationEvent
{
    public int Id { get; set; }

    public ContactCreatedIntegrationEvent(int id)
    {
        Id = id;
    }
}
