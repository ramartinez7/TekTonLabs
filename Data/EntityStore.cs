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
        ///     FindAsync an entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> GetByIdAsync(object id)
        {
            return DbEntitySet.FindAsync(id).AsTask();
        }

        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        public Task<EntityEntry<TEntity>> CreateAsync(TEntity entity)
        {
            return DbEntitySet.AddAsync(entity).AsTask();
        }

        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
