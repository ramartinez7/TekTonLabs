using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class EntityStore<TEntity, TKey> : IEntityStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public DbContext Context { get; private set; }
        private DbSet<TEntity> DbEntitySet { get; set; }

        public EntityStore(DatabaseContext context)
        {
            Context = context;
            DbEntitySet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> EntitySet()
        {
            return DbEntitySet;
        }

        /// <summary>
        ///     Get entity set
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await DbEntitySet.ToListAsync();
        }

        /// <summary>
        ///     FindAsync an entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await DbEntitySet.FindAsync(id);
        }

        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var value = await DbEntitySet.AddAsync(entity).AsTask();
            await Context.SaveChangesAsync();
            return value.Entity;
        }

        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
                await Context.SaveChangesAsync();
                return entity;
            }

            return null;
        }

        /// <summary>
        ///     Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool DeleteAsync(TKey id)
        {
            var entity = Activator.CreateInstance<TEntity>();
            entity.Id = id;

            DbEntitySet.Attach(entity);

            if (entity is not null)
            {
                DbEntitySet.Remove(entity);
                return true;
            }

            return false;
        }
    }
}
