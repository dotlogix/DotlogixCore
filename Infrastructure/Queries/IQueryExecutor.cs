// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IQueryExecutor.cs
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
    public interface IQueryExecutor<T> {
        #region To

        IAsyncEnumerable<T> ToAsyncEnumerable();
        IQueryable<T> ToQueryable();

        #endregion

        #region Any

        Task<bool> AnyAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region All

        Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region Count

        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<long> LongCountAsync(CancellationToken cancellationToken);
        Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region First

        Task<T> FirstAsync(CancellationToken cancellationToken);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region Last

        Task<T> LastAsync(CancellationToken cancellationToken);
        Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T> LastOrDefaultAsync(CancellationToken cancellationToken);
        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region Single

        Task<T> SingleAsync(CancellationToken cancellationToken);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region Min

        Task<T> MinAsync(CancellationToken cancellationToken);
        Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken);

        #endregion

        #region Max

        Task<T> MaxAsync(CancellationToken cancellationToken);
        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken);

        #endregion

        #region Sum

        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken);
        Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken);
        Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken);
        Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken);
        Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken);
        Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken);
        Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken);
        Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken);
        Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken);
        Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken);

        #endregion

        #region Average

        Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken);
        Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken);
        Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken);
        Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken);
        Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken);
        Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken);
        Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken);
        Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken);
        Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken);
        Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken);

        #endregion

        #region Misc

        Task<bool> ContainsAsync(T item, CancellationToken cancellationToken);

        #endregion
    }

    public class InterceptableQueryExecutor<T> : IQueryExecutor<T> {
        private readonly IQuery<T> _query;
        private readonly Func<IQuery<T>, IQueryExecutor<T>> _createExecutorFunc;

        public InterceptableQueryExecutor(IQuery<T> query, Func<IQuery<T>, IQueryExecutor<T>> createExecutorFunc) {
            _query = query;
            _createExecutorFunc = createExecutorFunc;
        }

        public IAsyncEnumerable<T> ToAsyncEnumerable() {
            return Intercept(query => _createExecutorFunc.Invoke(query).ToAsyncEnumerable());
        }

        private TResult Intercept<TResult>(Func<IQuery<T>, TResult> func) {
            var interceptors = _query.Interceptors.AsCollection();

            var query = interceptors.Aggregate(_query, (current, interceptor) => interceptor.BeforeExecute(current));
            var result = func.Invoke(query);
            return interceptors.Aggregate(result, (current, interceptor) => interceptor.AfterExecute(result));
        }

        public IQueryable<T> ToQueryable() {
            return Intercept((query)=> _createExecutorFunc.Invoke(query).ToQueryable());
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AnyAsync(cancellationToken));
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AnyAsync(predicate, cancellationToken));
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AllAsync(predicate, cancellationToken));
        }

        public Task<int> CountAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).CountAsync(cancellationToken));
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).CountAsync(predicate, cancellationToken));
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LongCountAsync(cancellationToken));
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LongCountAsync(predicate, cancellationToken));
        }

        public Task<T> FirstAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).FirstAsync(cancellationToken));
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).FirstAsync(predicate, cancellationToken));
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).FirstOrDefaultAsync(cancellationToken));
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).FirstOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> LastAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LastAsync(cancellationToken));
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LastAsync(predicate, cancellationToken));
        }

        public Task<T> LastOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LastOrDefaultAsync(cancellationToken));
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).LastOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> SingleAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SingleAsync(cancellationToken));
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SingleAsync(predicate, cancellationToken));
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SingleOrDefaultAsync(cancellationToken));
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SingleOrDefaultAsync(predicate, cancellationToken));
        }

        public Task<T> MinAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).MinAsync(cancellationToken));
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).MinAsync(selector, cancellationToken));
        }

        public Task<T> MaxAsync(CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).MaxAsync(cancellationToken));
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).MaxAsync(selector, cancellationToken));
        }

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<int?> SumAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<long?> SumAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<double?> SumAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<float?> SumAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).SumAsync(selector, cancellationToken));
        }

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, int?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, long?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<double?> AverageAsync(Expression<Func<T, double?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<float?> AverageAsync(Expression<Func<T, float?>> selector, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).AverageAsync(selector, cancellationToken));
        }

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken) {
            return Intercept((query)=>_createExecutorFunc.Invoke(query).ContainsAsync(item, cancellationToken));
        }
    }
}
