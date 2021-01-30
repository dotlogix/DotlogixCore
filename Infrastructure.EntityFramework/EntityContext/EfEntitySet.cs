using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Queries;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Repositories;
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
        public virtual Task<TEntity> AddAsync(TEntity entity) {
            var entry = DbSet.Add(entity);
            return Task.FromResult(entry.Entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.AsCollection();
            DbSet.AddRange(entities);
            return Task.FromResult(entities);
		}

        /// <inheritdoc />
        public virtual Task<TEntity> RemoveAsync(TEntity entity)
        {
	        var entry = DbSet.Remove(entity);
			return Task.FromResult(entry.Entity);
		}

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.AsCollection();
			DbSet.RemoveRange(entities);
            return Task.FromResult(entities);
		}

        /// <inheritdoc />
        public virtual Task<TEntity> ReAttachAsync(TEntity entity) {
            var entry = DbSet.Attach(entity);
            return Task.FromResult(entry.Entity);
		}

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            entities = entities.AsCollection();
            DbSet.AttachRange(entities);
            return Task.FromResult(entities);
		}

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(DbSet);
        }

        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        public virtual IQuery<TEntity> Query(params IQueryModifier<TEntity>[] filters) {
            var query = Query();
            switch (filters.Length)
            {
                case 0:
                    return query;
                case 1:
                    return filters[0].Apply(EntityContext, query);
                default:
                    return filters.Aggregate(query, (q, f) => f.Apply(EntityContext, q));
            }
        }
    }
}