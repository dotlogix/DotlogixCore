using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Infrastructure.EntityFramework.Hooks;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Infrastructure.EntityFramework {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, new() {
        private DbSet<TEntity> _dbSet;
        private IEfEventHandler _eventHandler;

        /// <summary>
        /// The entity context
        /// </summary>
        public IEfEntityContext EntityContext { get; }

        IEntityContext IEntitySet<TEntity>.EntityContext => EntityContext;

        /// <summary>
        /// The entity db set
        /// </summary>
        protected DbSet<TEntity> DbSet => _dbSet ??= EntityContext.DbContext.Set<TEntity>();

        /// <summary>
        /// The entity hooks
        /// </summary>
        protected IEfEventHandler EventHandler => _eventHandler ??= EntityContext.EventManager.GetHandler<TEntity>();


        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(IEfEntityContext entityContext) {
            EntityContext = entityContext;
        }
        
        /// <inheritdoc />
        public virtual Task<TEntity> AddAsync(TEntity entity) {
            var entry = DbSet.Add(entity);
            return Task.FromResult(entry.Entity);
        }

        /// <inheritdoc />
        public virtual Task<ICollection<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityCollection = entities.AsCollection();
            DbSet.AddRange(entityCollection);
            return Task.FromResult(entityCollection);
		}

        /// <inheritdoc />
        public virtual Task<TEntity> RemoveAsync(TEntity entity)
        {
	        var entry = DbSet.Remove(entity);
			return Task.FromResult(entry.Entity);
		}

        /// <inheritdoc />
        public virtual Task<ICollection<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityCollection = entities.AsCollection();
			DbSet.RemoveRange(entityCollection);
            return Task.FromResult(entityCollection);
		}

        /// <inheritdoc />
        public virtual TEntity Attach(TEntity entity) {
            DbSet.Attach(entity);
            return entity;
		}

        /// <inheritdoc />
        public virtual ICollection<TEntity> AttachRange(IEnumerable<TEntity> entities) {
            var entityCollection = entities.AsCollection();
            DbSet.AttachRange(entityCollection);
            return entityCollection;
		}

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Query() {
            return DbSet;
        }
    }
}