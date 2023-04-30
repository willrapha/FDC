namespace FDC.Generics.Domain
{
    public interface IUnitOfWork 
    {
        Task Commit();
    }
}
