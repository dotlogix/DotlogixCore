using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Options;
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
    public abstract class EfGuidEntitySet<TEntity> : EfEntitySet<TEntity> where TEntity : class, IGuid
    {
        private readonly DbSet<TEntity> _dbSet;

		/// <summary>
		/// Create a new instance of <see cref="EfGuidEntitySet{TEntity}"/>
		/// </summary>
		public EfGuidEntitySet(DbSet<TEntity> dbSet) : base(dbSet) {
        }

        /// <inheritdoc />
        public override ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default)
        {
	        var guid = (Guid)key;
	        var asyncResult = Query()
	                          .Where(e => e.Guid == guid)
	                          .FirstOrDefaultAsync(cancellationToken);
	        return new ValueTask<TEntity>(asyncResult);
        }

        /// <inheritdoc />
        public override ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys,
	        CancellationToken cancellationToken = default)
        {
	        var guids = keys.Cast<Guid>();
	        var asyncResult = Query()
	                          .Where(e => guids.Contains(e.Guid))
	                          .ToEnumerableAsync(cancellationToken);
	        return new ValueTask<IEnumerable<TEntity>>(asyncResult);
		}
    }
}