using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDC.Generics.Bus.Abstractations
{
    public interface IEventBus
    {
        /// <summary>
        /// Publish a message in direct queue.
        /// </summary>
        /// <param name="event">Is a Message</param>
        /// <param name="queueName">Is a Queue</param>
        /// <param name="exchangeName">Is a Exchange</param>
        void Publish(Event @event, string queueName, string exchangeName);

        /// <summary>
        /// Consumer of direct queue with deadletter queue option.
        /// </summary>
        /// <typeparam name="E">Is a Message Type</typeparam>
        /// <typeparam name="EH">Is a Consumer</typeparam>
        /// <param name="queueName">Is a Queue</param>
        /// <param name="exchangeName">Is a Exchange</param>
        /// <param name="prefetchCount">Limit of messages consumed at a time</param>
        /// <param name="deadLetter"></param>
        void Subscribe<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10, bool deadLetter = false)
            where E : Event
            where EH : IEventHandler<E>;

        /// <summary>
        /// Publish a message in fanout exchange.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="exchangeName"></param>
        void PublishInExchange(Event @event, string exchangeName);

        /// <summary>
        /// Consumer of queue with fanout exchange.
        /// </summary>
        /// <typeparam name="E">Is a Message Type</typeparam>
        /// <typeparam name="EH">Is a Consumer</typeparam>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="prefetchCount">Limit of messages consumed at a time</param>
        void SubscribeInExchange<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10)
           where E : Event
           where EH : IEventHandler<E>;

        /// <summary>
        /// Publish a message in direct queue with deadletter options.
        /// </summary>
        /// <param name="event">Is a Message</param>
        void Publish(Event @event, EventBusOptions options);

        /// <summary>
        /// Consumer of a Direct Queue with Deadletter, RoutingKey and TTL options.
        /// </summary>
        /// <typeparam name="E">Is a Message Type</typeparam>
        /// <typeparam name="EH">Is a Consumer</typeparam>
        /// <param name="options"></param>
        void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
           where E : Event
           where EH : IEventHandler<E>;
    }
}
