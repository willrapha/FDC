namespace FDC.Generics.Bus
{
    public sealed class EventBusOptions
    {
        public string ExchangeName { get; private set; }
        public string QueueName { get; private set; }
        public ushort? PrefetchCount { get; private set; } = 10;
        public bool WithDeadletter { get; private set; } = false;
        public bool WithDeadletterTimeToLive { get; private set; } = false;
        public ushort TimeToLiveInMinutes { get; private set; } = 5;

        public EventBusOptions() { }

        /// <summary>
        /// EventBusOptions with deadletter
        /// </summary>
        /// <remarks></remarks>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="withDeadletter"></param>
        /// <param name="prefetchCount"></param>
        /// <returns></returns>
        public static EventBusOptions Config(string exchangeName, string queueName, bool withDeadletter, ushort? prefetchCount = 10)
        {
            return new EventBusOptions
            {
                ExchangeName = exchangeName,
                QueueName = queueName,
                WithDeadletter = withDeadletter,
                PrefetchCount = prefetchCount
            };
        }

        /// <summary>
        /// EventBusOptions with deadletter and time to live options. Discards the message after timeToLiveInMinutes.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="withDeadletterTimeToLive"></param>
        /// <param name="timeToLiveInMinutes">Default is 5 minutes</param>
        /// <param name="prefetchCount">Default is 10</param>
        /// <returns></returns>
        public static EventBusOptions ConfigWithTimeToLive(string exchangeName, string queueName, bool withDeadletterTimeToLive, ushort timeToLiveInMinutes, ushort? prefetchCount = 10)
        {
            return new EventBusOptions
            {
                ExchangeName = exchangeName,
                QueueName = queueName,
                WithDeadletterTimeToLive = withDeadletterTimeToLive,
                TimeToLiveInMinutes = timeToLiveInMinutes,
                PrefetchCount = prefetchCount
            };
        }
    }
}
