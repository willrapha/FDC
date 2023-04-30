namespace FDC.Generics.Domain
{
    public class DomainNotificationService : IDomainNotificationService<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationService()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Add(DomainNotification message)
        {
            _notifications.Add(message);
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public void Clean()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Dispose()
        {
            Clean();
        }
    }
}
