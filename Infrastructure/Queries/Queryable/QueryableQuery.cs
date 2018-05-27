using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQuery<T> : IQuery<T>
    {
        private IQueryable<T> _innerQueryable;
        private readonly IQueryableQueryFactory _factory;
        private IQueryExecutor<T> _queryExecutor;

        public QueryableQuery(IQueryable<T> innerQueryable, IQueryableQueryFactory factory) {
            _innerQueryable = innerQueryable;
            _factory = factory;
        }

        public IQueryExecutor<T> QueryExecutor => _queryExecutor ?? (_queryExecutor = _factory.CreateQueryExecutor(_innerQueryable));

        public IQuery<T> Where(Expression<Func<T, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.Where(predicate);
            return this;
        }

        public IQuery<T> Where(Expression<Func<T, int, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.Where(predicate);
            return this;
        }

        public IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _factory.CreateQuery(_innerQueryable.Select(selector));
        }

        public IQuery<TResult> Select<TResult>(Expression<Func<T, int, TResult>> selector)
        {
            return _factory.CreateQuery(_innerQueryable.Select(selector));
        }

        public IQuery<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector)
        {
            return _factory.CreateQuery(_innerQueryable.SelectMany(selector));
        }

        public IQuery<TResult> SelectMany<TResult>(Expression<Func<T, int, IEnumerable<TResult>>> selector)
        {
            return _factory.CreateQuery(_innerQueryable.SelectMany(selector));
        }

        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<T, IEnumerable<TCollection>>> collectionSelector, Expression<Func<T, TCollection, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        public IQuery<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        public IQuery<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        public IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _factory.CreateOrderedQuery(_innerQueryable.OrderBy(keySelector));
        }

        public IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _factory.CreateOrderedQuery(_innerQueryable.OrderByDescending(keySelector));
        }

        public IQuery<T> Take(int count)
        {
            _innerQueryable = _innerQueryable.Take(count);
            return this;
        }

        public IQuery<T> TakeWhile(Expression<Func<T, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.TakeWhile(predicate);
            return this;
        }

        public IQuery<T> TakeWhile(Expression<Func<T, int, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.TakeWhile(predicate);
            return this;
        }

        public IQuery<T> Skip(int count)
        {
            _innerQueryable = _innerQueryable.Skip(count);
            return this;
        }

        public IQuery<T> SkipWhile(Expression<Func<T, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.SkipWhile(predicate);
            return this;
        }

        public IQuery<T> SkipWhile(Expression<Func<T, int, bool>> predicate)
        {
            _innerQueryable = _innerQueryable.SkipWhile(predicate);
            return this;
        }

        public IQuery<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _factory.CreateQuery(_innerQueryable.GroupBy(keySelector));
        }

        public IQuery<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector)
        {
            return _factory.CreateQuery(_innerQueryable.GroupBy(keySelector, elementSelector));
        }

        public IQuery<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.GroupBy(keySelector, elementSelector, resultSelector));
        }

        public IQuery<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector)
        {
            return _factory.CreateQuery(_innerQueryable.GroupBy(keySelector, resultSelector));
        }

        public IQuery<T> Distinct()
        {
            _innerQueryable = _innerQueryable.Distinct();
            return this;
        }

        public IQuery<T> Concat(IEnumerable<T> source2)
        {
            _innerQueryable = _innerQueryable.Concat(source2);
            return this;
        }

        public IQuery<T> Union(IEnumerable<T> source2)
        {
            _innerQueryable = _innerQueryable.Union(source2);
            return this;
        }

        public IQuery<T> Intersect(IEnumerable<T> source2)
        {
            _innerQueryable = _innerQueryable.Union(source2);
            return this;
        }

        public IQuery<T> Except(IEnumerable<T> source2)
        {
            _innerQueryable = _innerQueryable.Except(source2);
            return this;
        }

        public IQuery<T> Reverse()
        {
            _innerQueryable = _innerQueryable.Reverse();
            return this;
        }

        public virtual IAsyncEnumerable<T> ToAsyncEnumerable()
        {
            return _innerQueryable.ToAsyncEnumerable();
        }
    }
}