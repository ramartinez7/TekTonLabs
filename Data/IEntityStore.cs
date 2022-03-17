using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TKey">Type of entity identifier</typeparam>
    public interface IEntityStore<TEntity, TKey> 
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        DbContext Context { get; }
        IQueryable<TEntity> EntitySet();
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> UpdateAsync(TEntity entity);
        bool DeleteAsync(TKey id);
    }
}