namespace FDC.Generics.Infra
{
    public interface IRepositorioBase<TId, TEntity>
    {
        Task AdicionarAsync(TEntity obj);
        void Adicionar(TEntity obj);
        Task<IEnumerable<TEntity>> ListarAsync();
        Task<TEntity> ObterPorIdAsync(TId id);
        void Remover(TEntity obj);
        void Atualizar(TEntity obj);
    }
}
