using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, new() {
        private DbSet<TEntity> _dbSet;
        private EfEntityHooks _hooks;

        /// <summary>
        /// The entity context
        /// </summary>
        public IEfEntityContext EntityContext { get; }

        IEntityContext IEntitySet<TEntity>.EntityContext => EntityContext;

        /// <summary>
        /// The entity db set
        /// </summary>
        protected DbSet<TEntity> DbSet => _dbSet ?? (_dbSet = EntityContext.DbContext.Set<TEntity>());

        /// <summary>
        /// The entity hooks
        /// </summary>
        protected EfEntityHooks Hooks => _hooks ?? (_hooks = EntityContext.GetEntityHooks<TEntity>());


        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(IEfEntityContext entityContext) {
            EntityContext = entityContext;
        }
        
        /// <inheritdoc />
        public virtual async ValueTask<TEntity> AddAsync(TEntity entity) {
            var asyncResult = await DbSet.AddAsync(entity);
            return asyncResult.Entity;
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
			var asyncResult = DbSet.AddRangeAsync(collection)
			                        .ContinueWith(e => collection.AsEnumerable(), TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
			return new ValueTask<IEnumerable<TEntity>>(asyncResult);
		}

        /// <inheritdoc />
        public virtual ValueTask<TEntity> RemoveAsync(TEntity entity)
        {
	        var entry = DbSet.Remove(entity);
			return new ValueTask<TEntity>(entry.Entity);
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
			DbSet.RemoveRange(collection);
            return new ValueTask<IEnumerable<TEntity>>(collection);
		}

        /// <inheritdoc />
        public virtual ValueTask<TEntity> ReAttachAsync(TEntity entity) {
            var  entry = DbSet.Attach(entity);
            return new ValueTask<TEntity>(entry.Entity);
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            var collection = entities.AsCollection();
            DbSet.AttachRange(collection);
            return new ValueTask<IEnumerable<TEntity>>(collection);
		}

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(DbSet);
        }
    }
}