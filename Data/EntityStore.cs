using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EntityStore<TEntity> : IEntityStore<TEntity> where TEntity : class
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
    }
}
