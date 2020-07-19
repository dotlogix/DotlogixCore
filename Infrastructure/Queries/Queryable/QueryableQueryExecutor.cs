// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryableQueryExecutor.cs
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
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries {
    /// <summary>
    /// An implementation of the <see cref="IQueryExecutor{T}"/> interface using an <see cref="IQueryable{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryableQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQueryable<T> _innerQueryable;

        /// <summary>
        /// Creates a new instance of <see cref="QueryableQueryExecutor{T}"/>
        /// </summary>
        public QueryableQueryExecutor(IQuery<T> query) {
            _innerQueryable = (query as QueryableQuery<T>)?.InnerQueryable ?? throw new ArgumentException($"Query can not be converted to type {typeof(QueryableQuery<>).GetFriendlyName()}");
        }

        #region To

        /// <inheritdoc />
        public Task<List<T>> ToListAsync(CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToList());
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> ToEnumerableAsync(CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToList().AsEnumerable());
        }

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToArray());
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToDictionary(keySelector));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToDictionary(keySelector, comparer));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToDictionary(keySelector, elementSelector));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) {
            return Task.FromResult(_innerQueryable.ToDictionary(keySelector, elementSelector, comparer));
        }

        /// <inheritdoc />
        public IQueryable<T> ToQueryable()
        {
            return _innerQueryable;
        }

        #endregion

        #region Any

        /// <inheritdoc />
        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Any());
        }
        /// <inheritdoc />
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Any(predicate));
        }

        #endregion

        #region All

        /// <inheritdoc />
        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.All(predicate));
        }

        #endregion

        #region Count

        /// <inheritdoc />
        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Count());
        }

        /// <inheritdoc />
        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Count(predicate));
        }

        /// <inheritdoc />
        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LongCount());
        }

        /// <inheritdoc />
        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LongCount(predicate));
        }

        #endregion

        #region First

        /// <inheritdoc />
        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.First());
        }

        /// <inheritdoc />
        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.First(predicate));
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.FirstOrDefault());
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.FirstOrDefault(predicate));
        }

        #endregion

        #region Last

        /// <inheritdoc />
        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Last());
        }

        /// <inheritdoc />
        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Last(predicate));
        }

        /// <inheritdoc />
        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LastOrDefault());
        }

        /// <inheritdoc />
        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LastOrDefault(predicate));
        }

        #endregion

        #region Single

        /// <inheritdoc />
        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Single());
        }

        /// <inheritdoc />
        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Single(predicate));
        }

        /// <inheritdoc />
        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.SingleOrDefault());
        }

        /// <inheritdoc />
        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.SingleOrDefault(predicate));
        }

        #endregion

        #region Min

        /// <inheritdoc />
        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Min());
        }

        /// <inheritdoc />
        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Min(selector));
        }

        #endregion

        #region Max

        /// <inheritdoc />
        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Max());
        }

        /// <inheritdoc />
        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Max(selector));
        }

        #endregion

        #region Sum

        /// <inheritdoc />
        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        /// <inheritdoc />
        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        #endregion

        #region Average

        /// <inheritdoc />
        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        /// <inheritdoc />
        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        #endregion

        #region Misc

        /// <inheritdoc />
        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Contains(item));
        }

        #endregion
    }
}
