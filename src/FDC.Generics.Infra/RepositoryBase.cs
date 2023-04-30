using FDC.Generics.Domain;
using Microsoft.EntityFrameworkCore;

namespace FDC.Generics.Infra
{
    public class RepositoryBase<TId, TEntity> where TId : struct where TEntity : Entity<TId, TEntity>
    {
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }
        public async Task AdicionarAsync(TEntity obj) => await _dbSet.AddAsync(obj);
        public void Adicionar(TEntity obj) => _dbSet.Add(obj);

        public void Atualizar(TEntity obj) => _dbSet.Update(obj);

        public void Remover(TEntity obj) => _dbSet.Remove(obj);

        public async Task<IEnumerable<TEntity>> ListarAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity> ObterPorIdAsync(TId id) => await _dbSet.FindAsync(id);
    }
}