using static FDC.Generics.Bus.InMemory.InMemoryEventBusSubscriptionsManager;

namespace FDC.Generics.Bus.Abstractations
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>(string queue)
           where T : Event
           where TH : IEventHandler<T>;

        void RemoveSubscription<T, TH>()
             where TH : IEventHandler<T>
             where T : Event;

        bool HasSubscriptionsForEvent<T>() where T : Event;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);
        Type GetEventTypeByNameQueue(string queueName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();
    }
}
