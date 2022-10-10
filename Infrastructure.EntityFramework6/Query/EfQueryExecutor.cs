// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfQueryExecutor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;
#pragma warning disable 1591
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Query {
    /// <summary>
    /// An implementation of the <see cref="IQueryExecutor{T}"/> for entity framework
    /// </summary>
    public class EfQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQueryable<T> _innerQueryable;
        /// <summary>
        /// Create a new instance of <see cref="EfQueryExecutor{T}"/>
        /// </summary>
        public EfQueryExecutor(IQuery<T> query) {
            _innerQueryable = (query as QueryableQuery<T>)?.InnerQueryable ?? throw new ArgumentException($"Query can not be converted to type {typeof(QueryableQuery<>).GetFriendlyName()}");
        }

        public IAsyncEnumerable<T> ToAsyncEnumerable() {
            return _innerQueryable.ToAsyncEnumerable();
        }

        public IQueryable<T> ToQueryable() {
            return _innerQueryable;
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return _innerQueryable.AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.AnyAsync(predicate, cancellationToken);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.AnyAsync(predicate, cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return _innerQueryable.CountAsync(cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.CountAsync(predicate, cancellationToken);
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return _innerQueryable.LongCountAsync(cancellationToken);
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.LongCountAsync(predicate, cancellationToken);
        }

        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return _innerQueryable.FirstAsync(cancellationToken);
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.FirstAsync(predicate, cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return _innerQueryable.Reverse().FirstAsync(cancellationToken);
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Reverse().FirstAsync(predicate, cancellationToken);
        }

        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.Reverse().FirstOrDefaultAsync(cancellationToken);
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.Reverse().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return _innerQueryable.SingleAsync(cancellationToken);
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.SingleAsync(predicate, cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return _innerQueryable.SingleOrDefaultAsync(cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return _innerQueryable.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return _innerQueryable.MinAsync(cancellationToken);
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.MinAsync(selector, cancellationToken);
        }

        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return _innerQueryable.MaxAsync(cancellationToken);
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.MaxAsync(selector, cancellationToken);
        }

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.SumAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return _innerQueryable.AverageAsync(selector, cancellationToken);
        }

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return _innerQueryable.ContainsAsync(item, cancellationToken);
        }
    }
}
