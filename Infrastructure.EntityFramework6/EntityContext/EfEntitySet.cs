using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, ISimpleEntity {
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> AddAsync(TEntity entity) {
			return new ValueTask<TEntity>(_dbSet.Add(entity));
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return new ValueTask<IEnumerable<TEntity>>(_dbSet.AddRange(entities));
		}

        /// <inheritdoc />
        public virtual ValueTask<TEntity> RemoveAsync(TEntity entity) {
            return new ValueTask<TEntity>(_dbSet.Remove(entity));
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
	        return new ValueTask<IEnumerable<TEntity>>(_dbSet.RemoveRange(entities));
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> ReAttachAsync(TEntity entity) {
            return new ValueTask<TEntity>(_dbSet.Attach(entity));
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
	        return new ValueTask<IEnumerable<TEntity>>(entities.Select(_dbSet.Attach));
        }

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(_dbSet);
        }
    }
}