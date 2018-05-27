using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQueryable<T> _innerQueryable;
        public QueryableQueryExecutor(IQueryable<T> queryable) {
            _innerQueryable = queryable;
        }

        public IAsyncEnumerable<T> ToAsyncEnumerable() {
            return _innerQueryable.ToAsyncEnumerable();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Any(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().Any(cancellationToken);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().All(predicate.Compile(), cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Count(cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().Count(cancellationToken);
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().LongCount(cancellationToken);
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().LongCount(cancellationToken);
        }

        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().First(cancellationToken);
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().First(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().FirstOrDefault(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().FirstOrDefault(cancellationToken);
        }

        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Last(cancellationToken);
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().Last(cancellationToken);
        }

        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().LastOrDefault(cancellationToken);
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().LastOrDefault(cancellationToken);
        }

        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Single(cancellationToken);
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().Single(cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().SingleOrDefault(cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Where(predicate).ToAsyncEnumerable().SingleOrDefault(cancellationToken);
        }

        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Min(cancellationToken);
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Min(cancellationToken);
        }

        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Max(cancellationToken);
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Max(cancellationToken);
        }

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Sum(cancellationToken);
        }

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.Select(selector).ToAsyncEnumerable().Average(cancellationToken);
        }

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return _innerQueryable.ToAsyncEnumerable().Contains(item, cancellationToken);
        }
    }
}