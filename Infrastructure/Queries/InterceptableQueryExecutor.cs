using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;

namespace DotLogix.Architecture.Infrastructure.Queries {
    public class InterceptableQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQuery<T> _query;
        private readonly Func<IQuery<T>, IQueryExecutor<T>> _createExecutorFunc;

        public InterceptableQueryExecutor(IQuery<T> query, Func<IQuery<T>, IQueryExecutor<T>> createExecutorFunc) {
            _query = query;
            _createExecutorFunc = createExecutorFunc;
        }

        protected virtual async Task<TResult> Intercept<TResult>(Func<IQuery<T>, Task<TResult>> func) {
            var interceptors = _query.Interceptors.AsCollection();

            var context = QueryInterceptionContext.FromQuery<T, TResult>(_query);
            var cancelled = false;

            var state = "BeforeExecute";
            IQueryInterceptor cancellingInterceptor = null;
            foreach (var interceptor in interceptors) {
                if (interceptor.BeforeExecute(context))
                    continue;
                cancelled = true;
                cancellingInterceptor = interceptor;
                break;
            }

            if (!cancelled) {
                state = "Execute";
                context.DisableQueryModification();

                try {
                    var resultTask = func.Invoke((IQuery<T>)context.Query);
                    if(resultTask.Status != TaskStatus.RanToCompletion)
                        await resultTask;
                    context.Result = resultTask.Result;
                } catch (Exception e) {
                    state = "HandleError";
                    context.Exception = e;
                    foreach (var interceptor in interceptors) {
                        if (interceptor.HandleError(context))
                            continue;
                        cancelled = true;
                        cancellingInterceptor = interceptor;
                        break;
                    }
                }
            }

            if(!cancelled) {
                state = "AfterExecute";
                foreach (var interceptor in interceptors) {
                    if (interceptor.AfterExecute(context))
                        continue;
                    cancellingInterceptor = interceptor;
                    break;
                }
            }
            

            if (context.Success) {
                return context.Result.Value is TResult t ? t : default;
            }

            if (context.Faulted) {
                ExceptionDispatchInfo.Capture(context.Exception).Throw();
                return default;
            }

            if(cancellingInterceptor != null)
                throw new OperationCanceledException($"The query was cancelled by interceptor {cancellingInterceptor} in query state {state}.");

            // This should never happen
            throw new OperationCanceledException($"The query was cancelled by an interceptor for unknown reason in query state {state}.");
        }

        protected Task<TResult> Intercept<TResult>(Func<IQueryExecutor<T>, Task<TResult>> func) {
            return Intercept(query => func.Invoke(_createExecutorFunc.Invoke(query)));
        }

        protected TResult Intercept<TResult>(Func<IQuery<T>, TResult> func) {
            var task = Intercept((IQuery<T> query) => Task.FromResult(func.Invoke(_query)));
            return task.Result;
        }

        protected TResult Intercept<TResult>(Func<IQueryExecutor<T>, TResult> func) {
            var task = Intercept((IQuery<T> query) => Task.FromResult(func.Invoke(_createExecutorFunc.Invoke(_query))));
            return task.Result;
        }

        /// <inheritdoc />
        public Task<List<T>> ToListAsync(CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToListAsync(cancellationToken));
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> ToEnumerableAsync(CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToEnumerableAsync(cancellationToken));
        }

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToArrayAsync(cancellationToken));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToDictionaryAsync(keySelector, cancellationToken));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToDictionaryAsync(keySelector, comparer, cancellationToken));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToDictionaryAsync(keySelector, elementSelector, cancellationToken));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return Intercept((IQueryExecutor<T> executor) => executor.ToDictionaryAsync(keySelector, elementSelector, comparer, cancellationToken));
        }

        public IQueryable<T> ToQueryable() {
            return Intercept((IQueryExecutor<T> executor) => executor.ToQueryable());
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AnyAsync(cancellationToken));
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AnyAsync(predicate, cancellationToken));
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AllAsync(predicate, cancellationToken));
        }

        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.CountAsync(cancellationToken));
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.CountAsync(predicate, cancellationToken));
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LongCountAsync(cancellationToken));
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LongCountAsync(predicate, cancellationToken));
        }

        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.FirstAsync(cancellationToken));
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.FirstAsync(predicate, cancellationToken));
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.FirstOrDefaultAsync(cancellationToken));
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.FirstOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LastAsync(cancellationToken));
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LastAsync(predicate, cancellationToken));
        }

        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LastOrDefaultAsync(cancellationToken));
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.LastOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SingleAsync(cancellationToken));
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SingleAsync(predicate, cancellationToken));
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SingleOrDefaultAsync(cancellationToken));
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SingleOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.MinAsync(cancellationToken));
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.MinAsync(selector, cancellationToken));
        }

        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.MaxAsync(cancellationToken));
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.MaxAsync(selector, cancellationToken));
        }

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.SumAsync(selector, cancellationToken));
        }

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.AverageAsync(selector, cancellationToken));
        }

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return Intercept((IQueryExecutor<T> executor) => executor.ContainsAsync(item, cancellationToken));
        }
    }
}