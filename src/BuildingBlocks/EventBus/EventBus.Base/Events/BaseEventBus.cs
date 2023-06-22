using EventBus.Base.Abstraction;
using EventBus.Base.SubManager;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base.Events;

public abstract class BaseEventBus : IEventBus
{
    public readonly IServiceProvider ServiceProvider;
    public readonly IEventBusSubscriptionManager SubsManager;

    public EventBusConfig EventBusConfig;

    public BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
    {
        EventBusConfig = config;
        ServiceProvider = serviceProvider;
        SubsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    public virtual string ProcessEventName(string eventName)
    {
        // if (EventBusConfig.DeleteEventPrefix)
        // {
        //     eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());
        // }
        //
        // if (EventBusConfig.DeleteEventSuffix)
        // {
        //     var s = EventBusConfig.EventNameSuffix.ToArray();
        //     eventName = eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray());
        // }
        
        if(EventBusConfig.DeleteEventSuffix)
        {
            eventName = eventName.Replace(EventBusConfig.EventNameSuffix, "");
        }

        return eventName;
    }

    public virtual string GetSubName(string eventName)
    {
        return $"{EventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
    }

    public virtual void Dispose()
    {
        EventBusConfig = null;
    }

    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);
        var processed = false;

        if (SubsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = SubsManager.GetHandlersForEvent(eventName);
            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;
                    
                    var eventType = SubsManager.GetEventTypeByName($"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                    // if (integrationEvent is IntegrationEvent)
                    // {
                    //     eventBusConfig.CorrelationIdSetter?.Invoke((integrationEvent as IntegrationEvent)
                    //         .CorrelationId);
                    // }

                    var concrateType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concrateType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
            processed = true;
        }
        return processed;
    }

    public abstract void Publish(IntegrationEvent @event);

    public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    public abstract void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}