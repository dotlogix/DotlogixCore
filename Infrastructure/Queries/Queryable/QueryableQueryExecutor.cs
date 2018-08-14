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
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQueryable<T> _innerQueryable;

        public QueryableQueryExecutor(IQueryable<T> queryable) {
            _innerQueryable = queryable;
        }

        #region To

        public IAsyncEnumerable<T> ToAsyncEnumerable() {
            return _innerQueryable.ToAsyncEnumerable();
        }

        public IQueryable<T> ToQueryable()
        {
            return _innerQueryable;
        }

        #endregion

        #region Any

        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Any());
        }
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Any(predicate));
        }

        #endregion

        #region All

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.All(predicate));
        }

        #endregion

        #region Count

        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Count());
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Count(predicate));
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LongCount());
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LongCount(predicate));
        }

        #endregion

        #region First

        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.First());
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.First(predicate));
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.FirstOrDefault());
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.FirstOrDefault(predicate));
        }

        #endregion

        #region Last

        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Last());
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Last(predicate));
        }

        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LastOrDefault());
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.LastOrDefault(predicate));
        }

        #endregion

        #region Single

        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Single());
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Single(predicate));
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.SingleOrDefault());
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.SingleOrDefault(predicate));
        }

        #endregion

        #region Min

        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Min());
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Min(selector));
        }

        #endregion

        #region Max

        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Max());
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Max(selector));
        }

        #endregion

        #region Sum

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Sum(selector));
        }

        #endregion

        #region Average

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Average(selector));
        }

        #endregion

        #region Misc

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return Task.FromResult(_innerQueryable.Contains(item));
        }

        #endregion
    }
}
