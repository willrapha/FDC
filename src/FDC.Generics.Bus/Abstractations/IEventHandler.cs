namespace FDC.Generics.Bus.Abstractations
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        /// <summary>
        /// Invoked when there is a registered consumer.
        /// </summary>
        /// <param name="event">Is a message</param>
        /// <returns>Success or fail</returns>
        Task<bool> Handle(TEvent @event);
    }

    public interface IEventHandler
    {
    }
}
