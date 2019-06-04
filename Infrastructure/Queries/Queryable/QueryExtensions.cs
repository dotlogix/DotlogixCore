// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Tracking.Snapshots;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public interface IQueryInterceptor {
        IQuery<TValue> BeforeExecute<TValue>(IQuery<TValue> query);
        TResult AfterExecute<TResult>(TResult result);
    }

    public class QueryInterceptor : IQueryInterceptor {
        private Func<object, object> BeforeExecuteFunc { get; }
        private Func<object, object> AfterExecuteFunc { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public QueryInterceptor(Func<object, object> beforeExecuteFunc, Func<object, object> afterExecuteFunc) {
            BeforeExecuteFunc = beforeExecuteFunc;
            AfterExecuteFunc = afterExecuteFunc;
        }

        public IQuery<TValue> BeforeExecute<TValue>(IQuery<TValue> query) {
            return BeforeExecuteFunc != null ? (IQuery<TValue>)BeforeExecuteFunc(query) : query;
        }

        public TResult AfterExecute<TResult>(TResult result) {
            return AfterExecuteFunc != null ? (TResult)AfterExecuteFunc(result) : result;
        }
    }


    public static class QueryExtensions {
        #region Any

        public static Task<bool> AnyAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AnyAsync(cancellationToken);
        }

        public static Task<bool> AnyAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AnyAsync(predicate, cancellationToken);
        }

        #endregion

        #region All

        public static Task<bool> AllAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AllAsync(predicate, cancellationToken);
        }

        #endregion

        #region Count

        public static Task<int> CountAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.CountAsync(cancellationToken);
        }

        public static Task<int> CountAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.CountAsync(predicate, cancellationToken);
        }

        public static Task<long> LongCountAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LongCountAsync(cancellationToken);
        }

        public static Task<long> LongCountAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LongCountAsync(predicate, cancellationToken);
        }

        #endregion

        #region First

        public static Task<TSource> FirstAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.FirstAsync(cancellationToken);
        }

        public static Task<TSource> FirstAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.FirstAsync(predicate, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.FirstOrDefaultAsync(cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        #endregion

        #region Last

        public static Task<TSource> LastAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LastAsync(cancellationToken);
        }

        public static Task<TSource> LastAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LastAsync(predicate, cancellationToken);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LastOrDefaultAsync(cancellationToken);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.LastOrDefaultAsync(predicate, cancellationToken);
        }

        #endregion

        #region Single

        public static Task<TSource> SingleAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SingleAsync(cancellationToken);
        }

        public static Task<TSource> SingleAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SingleAsync(predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SingleOrDefaultAsync(cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        #endregion

        #region Min

        public static Task<TSource> MinAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.MinAsync(cancellationToken);
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IQuery<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.MinAsync(selector, cancellationToken);
        }

        #endregion

        #region Max

        public static Task<TSource> MaxAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.MaxAsync(cancellationToken);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IQuery<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.MaxAsync(selector, cancellationToken);
        }

        #endregion

        #region Sum

        public static Task<decimal> SumAsync(this IQuery<decimal> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<decimal?> SumAsync(this IQuery<decimal?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<decimal> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<decimal?> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<int> SumAsync(this IQuery<int> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<int?> SumAsync(this IQuery<int?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<int> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<int?> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<long> SumAsync(this IQuery<long> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<long?> SumAsync(this IQuery<long?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<long> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<long?> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<double> SumAsync(this IQuery<double> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<double?> SumAsync(this IQuery<double?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<double> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<double?> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<float> SumAsync(this IQuery<float> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<float?> SumAsync(this IQuery<float?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(v => v, cancellationToken);
        }

        public static Task<float> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        public static Task<float?> SumAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.SumAsync(selector, cancellationToken);
        }

        #endregion

        #region Average

        public static Task<decimal> AverageAsync(this IQuery<decimal> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<decimal?> AverageAsync(this IQuery<decimal?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<decimal> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<decimal?> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double> AverageAsync(this IQuery<int> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double?> AverageAsync(this IQuery<int?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double> AverageAsync(this IQuery<long> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double?> AverageAsync(this IQuery<long?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double> AverageAsync(this IQuery<double> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double?> AverageAsync(this IQuery<double?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<double> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<float> AverageAsync(this IQuery<float> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<float?> AverageAsync(this IQuery<float?> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(v => v, cancellationToken);
        }

        public static Task<float> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        public static Task<float?> AverageAsync<TSource>(this IQuery<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.AverageAsync(selector, cancellationToken);
        }

        #endregion

        #region To

        public static Task<List<TSource>> ToListAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToList(cancellationToken);
        }

        public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToList(cancellationToken).ConvertResult<IEnumerable<TSource>, List<TSource>>();
        }

        public static Task<TSource[]> ToArrayAsync<TSource>(this IQuery<TSource> source, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToArray(cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQuery<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToDictionary(keySelector, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQuery<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToDictionary(keySelector, comparer, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToDictionary(keySelector, elementSelector, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ToDictionary(keySelector, elementSelector, comparer, cancellationToken);
        }

        public static IQueryable<T> ToQueryable<T>(this IQuery<T> source)
        {
            return source.QueryExecutor.ToQueryable();
        }

        #endregion

        #region Misc

        public static Task<bool> ContainsAsync<TSource>(this IQuery<TSource> source, TSource item, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ContainsAsync(item, cancellationToken);
        }

        public static Task ForEachAsync<T>(this IQuery<T> source, Action<T> action, CancellationToken cancellationToken = default) {
            return source.QueryExecutor.ToAsyncEnumerable().ForEachAsync(action, cancellationToken);
        }

        public static IQuery<T> InterceptQuery<T>(this IQuery<T> query, Func<object, object> beforeExecutFunc, Func<object, object> afterExecuteFunc) {
            return query.InterceptQuery(new QueryInterceptor(beforeExecutFunc, afterExecuteFunc));
        }
        
        public static IQuery<T> InterceptQueryBeforeExecute<T>(this IQuery<T> query, Func<object, object> interceptor) {
            return query.InterceptQuery(new QueryInterceptor(interceptor, null));
        }
        public static IQuery<T> InterceptQueryResult<T>(this IQuery<T> query, Func<object, object> interceptor) {
            return query.InterceptQuery(new QueryInterceptor(null, interceptor));
        }

        #endregion
    }
}
