// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IQuery.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#pragma warning disable 1591
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries {
    public interface IQuery<T> {
        IQueryExecutor<T> QueryExecutor { get; }
        IEnumerable<IQueryInterceptor> Interceptors { get; }
        IDictionary<string, object> Variables { get; }

        #region Where

        IQuery<T> Where(Expression<Func<T, bool>> predicate);
        IQuery<T> Where(Expression<Func<T, int, bool>> predicate);

        #endregion

        #region Select

        IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQuery<TResult> Select<TResult>(Expression<Func<T, int, TResult>> selector);
        IQuery<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector);
        IQuery<TResult> SelectMany<TResult>(Expression<Func<T, int, IEnumerable<TResult>>> selector);
        IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector);
        IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector);

        #endregion

        #region Join

        IQuery<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector);
        IQuery<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, IEnumerable<TInner>, TResult>> resultSelector);

        #endregion

        #region OrderBy

        IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

        #endregion

        #region Take

        IQuery<T> Take(int count);
        IQuery<T> TakeWhile(Expression<Func<T, bool>> predicate);
        IQuery<T> TakeWhile(Expression<Func<T, int, bool>> predicate);

        #endregion

        #region Skip

        IQuery<T> Skip(int count);
        IQuery<T> SkipWhile(Expression<Func<T, bool>> predicate);
        IQuery<T> SkipWhile(Expression<Func<T, int, bool>> predicate);

        #endregion

        #region GroupBy

        IQuery<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IQuery<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector);
        IQuery<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector);
        IQuery<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector);

        #endregion

        #region Misc

        IQuery<T> Distinct();
        IQuery<T> Concat(IEnumerable<T> source2);
        IQuery<T> Union(IEnumerable<T> source2);
        IQuery<T> Intersect(IEnumerable<T> source2);
        IQuery<T> Except(IEnumerable<T> source2);
        IQuery<T> Reverse();

        #endregion

        #region Intercept
        IQuery<T> InterceptQuery(IQueryInterceptor interceptor);
        #endregion
    }
}
