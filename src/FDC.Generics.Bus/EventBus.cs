using Autofac;
using FDC.Generics.Bus.Abstractations;
using FDC.Generics.Bus.RabbitMQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace FDC.Generics.Bus
{
    public class EventBus : IEventBus, IDisposable
    {
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IRabbitMQConnection _rabbitConnection;
        private readonly ILifetimeScope _autofac;
        private readonly ILogger<EventBus> _logger = null;

        private const string AUTOFAC_SCOPE_NAME = "senac_generics_event_bus";

        private IModel _consumerChannel;

        public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection)
        {
            _subsManager = subsManager;
            _rabbitConnection = rabbitConnection;
        }

        public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection, ILogger<EventBus> logger)
        {
            _subsManager = subsManager;
            _rabbitConnection = rabbitConnection;
            _logger = logger;
        }

        public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection, ILogger<EventBus> logger, ILifetimeScope autofac) : this(subsManager, rabbitConnection, logger)
        {
            _autofac = autofac;
        }

        public void Publish(Event @event, string queueName, string exchangeName = "")
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();

            var policy = CreatePolicyRetry();

            using (var channel = _rabbitConnection.CreateModel())
            {
                DeclareExchangeDirect(channel, exchangeName);
                DeclareQueue(channel, queueName, null);
                BindQueue(channel, exchangeName, queueName, queueName);

                policy.Execute((Action)(() =>
                {
                    PublishMessage(channel, @event, queueName, exchangeName);
                }));
            }
        }

        public void Publish(Event @event, EventBusOptions options)
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();

            var policy = CreatePolicyRetry();

            using (var channel = _rabbitConnection.CreateModel())
            {
                var arguments = new Dictionary<string, Object>();
                bool isDeadletter = options.WithDeadletter || options.WithDeadletterTimeToLive;

                if (isDeadletter)
                    arguments = CreateDeadLetterQueue(channel, options);

                DeclareExchangeDirect(channel, options.ExchangeName);
                DeclareQueue(channel, options.QueueName, arguments);
                BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);

                policy.Execute((Action)(() =>
                {
                    PublishMessage(channel, @event, options.QueueName, options.ExchangeName);
                }));
            }
        }

        public void PublishInExchange(Event @event, string exchangeName)
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();

            var policy = CreatePolicyRetry();

            using (var channel = _rabbitConnection.CreateModel())
            {
                DeclareExchangeFanOut(channel, exchangeName);

                policy.Execute((Action)(() =>
                {
                    PublishMessageInExchange(channel, @event, exchangeName);
                }));
            }
        }

        public bool HasMessage(string queueName)
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();

            using (var channel = _rabbitConnection.CreateModel())
            {
                var result = channel.BasicGet(queueName, false);
                return result.MessageCount > 0;
            }
        }

        public void Subscribe<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10, bool deadLetter = false)
            where E : Event
            where EH : IEventHandler<E>
        {
            var eventName = _subsManager.GetEventKey<E>();

            _subsManager.AddSubscription<E, EH>(queueName);

            DoInternalSubscriptionByQueue(queueName, exchangeName, prefetchCount, deadLetter);
        }

        public void SubscribeInExchange<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10)
           where E : Event
           where EH : IEventHandler<E>
        {
            _subsManager.AddSubscription<E, EH>(exchangeName);

            DoInternalSubscriptionByExchange(queueName, exchangeName, prefetchCount);
        }

        public void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
           where E : Event
           where EH : IEventHandler<E>
        {
            _subsManager.AddSubscription<E, EH>(options.QueueName);

            CreateRabbitMQListenerDeadletter(options);
        }

        private void DoInternalSubscriptionByQueue(string queueName, string exchangeName, ushort? prefetchCount, bool deadLetter)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(queueName);

            if (!containsKey)
            {
                if (!_rabbitConnection.IsConnected)
                {
                    _rabbitConnection.TryConnect();
                }

                using (var channel = _rabbitConnection.CreateModel())
                {
                    BindQueue(channel, exchangeName, queueName, queueName);
                }
            }

            CreateRabbitMQListener(queueName, exchangeName, prefetchCount, deadLetter);
        }

        private void DoInternalSubscriptionByExchange(string queueName, string exchangeName, ushort? prefetchCount)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(exchangeName);

            if (!containsKey)
            {
                if (!_rabbitConnection.IsConnected)
                    _rabbitConnection.TryConnect();

                using (var channel = _rabbitConnection.CreateModel())
                {
                    DeclareExchangeFanOut(channel, exchangeName);
                    BindQueue(channel, exchangeName, queueName, queueName);
                }
            }

            CreateRabbitMQListenerExchange(queueName, exchangeName, prefetchCount);
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            try
            {
                if (_subsManager.HasSubscriptionsForEvent(eventName))
                {
                    using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                    {
                        var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                        foreach (var subscription in subscriptions)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);

                            if (handler == null)
                                continue;

                            var eventType = _subsManager.GetEventTypeByNameQueue(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                            return await (Task<bool>)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogCritical(ex, "Erro ao processar evento na camada Generics.Bus");
                }

                return false;
            }

            return false;
        }

        private IModel CreateRabbitMQListener(string queueName, string exchangeName, ushort? prefetchCount, bool deadLetter = false)
        {
            if (!_rabbitConnection.IsConnected)
            {
                _rabbitConnection.TryConnect();
            }

            var channel = _rabbitConnection.CreateModel();

            if (prefetchCount != null)
                channel.BasicQos(prefetchSize: 0, prefetchCount: prefetchCount.Value, global: false);

            var arguments = new Dictionary<string, Object>();

            if (deadLetter)
                arguments = CreateDeadLetterQueue(
                    channel,
                    EventBusOptions.Config(exchangeName, queueName, deadLetter, prefetchCount));

            DeclareExchangeDirect(channel, exchangeName);
            DeclareQueue(channel, queueName, arguments);
            BindQueue(channel, exchangeName, queueName, queueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.Span);

                var success = await ProcessEvent(eventName, message);

                if (success)
                    channel.BasicAck(ea.DeliveryTag, multiple: false);

                if (!success && deadLetter)
                {
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                    _logger.LogCritical($"Queue: {queueName} - Message: {message}");
                }
            };

            DeclareConsumer(channel, consumer, queueName);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateRabbitMQListener(queueName, exchangeName, prefetchCount);
            };

            return channel;
        }

        private IModel CreateRabbitMQListenerDeadletter(EventBusOptions options)
        {
            if (!_rabbitConnection.IsConnected)
            {
                _rabbitConnection.TryConnect();
            }

            var channel = _rabbitConnection.CreateModel();

            if (options.PrefetchCount != null)
                channel.BasicQos(prefetchSize: 0, prefetchCount: options.PrefetchCount.Value, global: false);

            var arguments = new Dictionary<string, Object>();
            bool isDeadletter = options.WithDeadletter || options.WithDeadletterTimeToLive;

            if (isDeadletter)
                arguments = CreateDeadLetterQueue(channel, options);

            DeclareExchangeDirect(channel, options.ExchangeName);
            DeclareQueue(channel, options.QueueName, arguments);
            BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.Span);
                
                var success = await ProcessEvent(eventName, message);

                if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.TryGetValue("x-death", out object xdeath))
                {
                    var xDeathObjects = xdeath as List<object>;
                    var countObject = xDeathObjects[0] as IDictionary<string, object>;

                    countObject.TryGetValue("count", out object value);

                    var count = Convert.ToInt32(value);

                    if (count >= 3)
                    {
                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    else
                    {
                        if (success)
                            channel.BasicAck(ea.DeliveryTag, multiple: false);

                        if (!success && isDeadletter)
                        {
                            channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                            _logger.LogCritical($"Queue: {options.QueueName} | Attempt: {count} | Message: {message}");
                        }
                    }
                }
                else
                {
                    if (success)
                        channel.BasicAck(ea.DeliveryTag, multiple: false);

                    if (!success && isDeadletter)
                    {
                        channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                        _logger.LogCritical($"Queue: {options.QueueName} | Message: {message}");
                    }
                }
            };

            DeclareConsumer(channel, consumer, options.QueueName);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateRabbitMQListenerDeadletter(options);
            };

            return channel;
        }

        private Dictionary<string, object> CreateDeadLetterQueue(IModel channel, EventBusOptions options)
        {
            string deadLetterQueueName = $"{options.QueueName}.dead-letter";
            string deadLetterExchangeName = "dead-letter-exchange";

            var argumentsDeadletter = new Dictionary<string, Object>();

            if (options.WithDeadletterTimeToLive)
            {
                argumentsDeadletter.Add("x-dead-letter-exchange", options.ExchangeName);
                argumentsDeadletter.Add("x-dead-letter-routing-key", options.QueueName);
                argumentsDeadletter.Add("x-message-ttl", (int)TimeSpan.FromMinutes(options.TimeToLiveInMinutes).TotalMilliseconds);
            }

            DeclareExchangeDirect(channel, deadLetterExchangeName);
            DeclareQueue(channel, deadLetterQueueName, argumentsDeadletter);
            BindQueue(channel, deadLetterExchangeName, deadLetterQueueName, deadLetterQueueName);

            var arguments = new Dictionary<string, Object>()
            {
                {"x-dead-letter-exchange",  deadLetterExchangeName },
                {"x-dead-letter-routing-key", deadLetterQueueName },
            };

            return arguments;
        }

        private IModel CreateRabbitMQListenerExchange(string queueName, string exchangeName, ushort? prefetchCount)
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();

            var channel = _rabbitConnection.CreateModel();

            if (prefetchCount != null)
                channel.BasicQos(prefetchSize: 0, prefetchCount: prefetchCount.Value, global: false);

            DeclareExchangeFanOut(channel, exchangeName);
            DeclareQueue(channel, queueName, null);
            BindQueue(channel, exchangeName, queueName, queueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.Span);

                var success = await ProcessEvent(eventName, message);

                if (success)
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            DeclareConsumer(channel, consumer, queueName);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateRabbitMQListener(queueName, exchangeName, prefetchCount);
            };

            return channel;
        }

        private static void PublishMessage(IModel channel, Event @event, string queueName, string exchangeName)
        {
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            var message = JsonConvert.SerializeObject(@event);
            var messageBody = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchangeName, queueName, true, properties, messageBody);
        }

        private static void PublishMessageInExchange(IModel channel, Event @event, string exchangeName)
        {
            var message = JsonConvert.SerializeObject(@event);
            var messageBody = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            properties.Persistent = true;

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: exchangeName,
                                 basicProperties: properties,
                                 body: messageBody);
        }

        private static void DeclareExchangeDirect(IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }

        private static void DeclareExchangeFanOut(IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);
        }

        private static void DeclareQueue(IModel channel, string queueName, Dictionary<string, Object> arguments)
        {
            channel.QueueDeclare(queueName, true, false, false, arguments);
        }

        private static void BindQueue(IModel channel, string exchangeName, string queueName, string routingKey)
        {
            channel.QueueBind(queueName, exchangeName, routingKey, null);
        }

        private static void DeclareConsumer(IModel channel, EventingBasicConsumer consumer, string queueName)
        {
            channel.BasicConsume(queueName, false, consumer);
        }

        private static RetryPolicy CreatePolicyRetry()
        {
            return RetryPolicy.Handle<BrokerUnreachableException>()
                              .Or<SocketException>()
                              .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
        }

        public void Dispose()
        {

        }

    }
}
