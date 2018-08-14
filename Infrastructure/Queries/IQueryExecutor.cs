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
}
