using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default) {
	        throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys, CancellationToken cancellationToken = default) {
	        throw new NotSupportedException();

		}

		/// <inheritdoc />
		public virtual ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
	        var asyncResult = Query()
	                          .ToEnumerableAsync(cancellationToken);
	        return new ValueTask<IEnumerable<TEntity>>(asyncResult);
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
	        var asyncResult = Query()
	                          .Where(filterExpression)
	                          .ToEnumerableAsync(cancellationToken);
	        return new ValueTask<IEnumerable<TEntity>>(asyncResult);
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> AddAsync(TEntity entity)
        {
	        var asyncResult = _dbSet.AddAsync(entity)
	                                .ContinueWith(e => e.Result.Entity, TaskContinuationOptions.OnlyOnRanToCompletion);
			return new ValueTask<TEntity>(asyncResult);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
			var asyncResult = _dbSet.AddRangeAsync(collection)
			                        .ContinueWith(e => collection.AsEnumerable(), TaskContinuationOptions.OnlyOnRanToCompletion);
			return new ValueTask<IEnumerable<TEntity>>(asyncResult);
		}

        /// <inheritdoc />
        public virtual ValueTask<TEntity> RemoveAsync(TEntity entity)
        {
	        var entry = _dbSet.Remove(entity);
			return new ValueTask<TEntity>(entry.Entity);
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
			_dbSet.RemoveRange(collection);
            return new ValueTask<IEnumerable<TEntity>>(collection);
		}

        /// <inheritdoc />
        public virtual ValueTask<TEntity> ReAttachAsync(TEntity entity) {
            var  entry = _dbSet.Attach(entity);
            return new ValueTask<TEntity>(entry.Entity);
		}

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            var collection = entities.AsCollection();
            _dbSet.AttachRange(collection);
            return new ValueTask<IEnumerable<TEntity>>(collection);
		}

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(_dbSet);
        }
    }
}