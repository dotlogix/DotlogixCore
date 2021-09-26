using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Infrastructure.Queries;
using DotLogix.Infrastructure.Repositories;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Terms;

namespace DotLogix.WebServices.EntityFramework.Repositories {
    public interface IGenericRepository<in TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class, new() {
    
        /// <inheritdoc cref="IRepository{TKey,TEntity}.GetAsync" />
        Task<TEntity> GetAsync(TKey key, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IRepository{TKey,TEntity}.GetRangeAsync" />
        Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        Task<PaginationResult<TEntity>> GetPagedAsync(PaginationTerm pagination, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        Task<PaginationResult<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filterExpression, PaginationTerm pagination, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        Task<PaginationResult<TEntity>> GetPagedAsync(IQueryModifier<TEntity, TEntity> queryModifier, PaginationTerm pagination, CancellationToken cancellationToken = default);
    }
}
