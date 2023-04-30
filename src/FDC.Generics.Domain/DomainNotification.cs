namespace FDC.Generics.Domain
{
    public class DomainNotification
    {
        public string Key { get; }
        public string Value { get; }

        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
