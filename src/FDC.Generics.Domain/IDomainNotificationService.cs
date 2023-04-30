namespace FDC.Generics.Domain
{
    public interface IDomainNotificationService<T>
    {
        bool HasNotifications();
        List<T> GetNotifications();
        void Clean();
        void Add(DomainNotification message);
    }
}
