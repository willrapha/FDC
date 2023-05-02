namespace FDC.Generics.Bus
{
    public abstract class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreateAt = DateTime.Now;
        }

        public Guid Id { get; }
        public DateTime CreateAt { get; }
    }
}
