using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Z.EntityFramework.Plus;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, new() {
        /// <summary>
        /// The inner DbSet
        /// </summary>
        protected DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(DbSet<TEntity> dbSet) {
            DbSet = dbSet;
        }
        
        /// <inheritdoc />
        public virtual async ValueTask<TEntity> AddAsync(TEntity entity) {
            var asyncResult = DbSet.AddAsync(entity);
            if(asyncResult.IsCompletedSuccessfully == false)
                await asyncResult;
            return asyncResult.Result.Entity;
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