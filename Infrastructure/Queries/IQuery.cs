using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Architecture.Infrastructure.Queries {
    public interface IQuery<T>
    {
        IQueryExecutor<T> QueryExecutor { get; }
        IQuery<T> Where(Expression<Func<T, bool>> predicate);
        IQuery<T> Where(Expression<Func<T, int, bool>> predicate);
        IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQuery<TResult> Select<TResult>(Expression<Func<T, int, TResult>> selector);
        IQuery<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector);
        IQuery<TResult> SelectMany<TResult>(Expression<Func<T, int, IEnumerable<TResult>>> selector);
        IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector);
        IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector);
        IQuery<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector);
        IQuery<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, IEnumerable<TInner>, TResult>> resultSelector);
        IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
        IQuery<T> Take(int count);
        IQuery<T> TakeWhile(Expression<Func<T, bool>> predicate);
        IQuery<T> TakeWhile(Expression<Func<T, int, bool>> predicate);
        IQuery<T> Skip(int count);
        IQuery<T> SkipWhile(Expression<Func<T, bool>> predicate);
        IQuery<T> SkipWhile(Expression<Func<T, int, bool>> predicate);
        IQuery<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IQuery<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector);
        IQuery<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector);
        IQuery<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector);
        IQuery<T> Distinct();
        IQuery<T> Concat(IEnumerable<T> source2);
        IQuery<T> Union(IEnumerable<T> source2);
        IQuery<T> Intersect(IEnumerable<T> source2);
        IQuery<T> Except(IEnumerable<T> source2);
        IQuery<T> Reverse();


    }
}