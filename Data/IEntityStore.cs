using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data
{
    public interface IEntityStore<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        IQueryable<TEntity> EntitySet();
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}