using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data
{
    public interface IEntityStore<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        IQueryable<TEntity> EntitySet();
        Task<EntityEntry<TEntity>> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        void Update(TEntity entity);
    }
}