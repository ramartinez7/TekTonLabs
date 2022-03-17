using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;

namespace Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TKey">Type of entity identifier</typeparam>
    public interface IEntityStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        DbContext Context { get; }
        IQueryable<TEntity> EntitySet();
        Task<IEnumerable<TEntity>> GetAsync();
        Task<bool> ExistsAsync(TKey id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TEntity> UpdateAsync(TEntity entity);
        bool DeleteAsync(TKey id);
    }
}