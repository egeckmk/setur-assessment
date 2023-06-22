using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTests.Events.EventHandlers;
using EventBus.UnitTests.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBus.UnitTests;

[TestClass]
public class EventBusTest
{
    private ServiceCollection services;

    public EventBusTest()
    {
        services = new ServiceCollection();
        services.AddLogging(configure => configure.AddConsole());
    }
    
    [TestMethod]
    public void subscribe_event_on_rabbitmq_test()
    {
        services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusFactory.Create(GetRabbitMQConfig(), sp);
        });
        
        var sp = services.BuildServiceProvider();

        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Subscribe<ContactCreatedIntegrationEvent, ContactCreatedIntegrationEventHandler>();
        // eventBus.UnSubscribe<ContactCreatedIntegrationEvent, ContactCreatedIntegrationEventHandler>();
    }

    [TestMethod]
    public void send_message_to_rabbitmq()
    {
        services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusFactory.Create(GetRabbitMQConfig(), sp);
        });
        
        var sp = services.BuildServiceProvider();

        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Publish(new ContactCreatedIntegrationEvent(1));
    }
    
    private EventBusConfig GetRabbitMQConfig()
    {
        return new EventBusConfig()
        {
            ConnectionRetryCount = 5,
            SubscriberClientAppName = "EventBus.UnitTest",
            DefaultTopicName = "PhoneBookTopicName",
            EventBusType = EventBusType.RabbitMQ,
            EventNameSuffix = "IntegrationEvent"
        };
    }
}