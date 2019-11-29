using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext
{
	/// <summary>
	/// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
	/// </summary>
	public abstract class EfIdEntitySet<TEntity> : EfEntitySet<TEntity> where TEntity : class, IIdentity
	{
		private readonly DbSet<TEntity> _dbSet;

		/// <summary>
		/// Create a new instance of <see cref="EfIdEntitySet{TEntity}"/>
		/// </summary>
		public EfIdEntitySet(DbSet<TEntity> dbSet) : base(dbSet) {
		}

		/// <inheritdoc />
		public override ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default)
		{
			var id = (int)key;
			var asyncResult = Query()
			                  .Where(e => e.Id == id)
			                  .FirstOrDefaultAsync(cancellationToken);
			return new ValueTask<TEntity>(asyncResult);
		}

		/// <inheritdoc />
		public override ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys,
			CancellationToken cancellationToken = default)
		{
			var ids = keys.Cast<int>();
			var asyncResult = Query()
			                  .Where(e => ids.Contains(e.Id))
			                  .ToEnumerableAsync(cancellationToken);
			return new ValueTask<IEnumerable<TEntity>>(asyncResult);
		}
	}
}