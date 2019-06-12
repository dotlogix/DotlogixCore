// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryableQuery.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    /// <summary>
    /// An implementation of the <see cref="IQuery{T}"/> interface using <see cref="IQueryable{T}"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class QueryableQuery<TValue> : IQuery<TValue> {
        private readonly IQueryableQueryFactory _factory;
        private IQueryExecutor<TValue> _queryExecutor;

        /// <summary>
        /// Creates a new instance of <see cref="QueryableQuery{TValue}"/>
        /// </summary>
        public QueryableQuery(IQueryable<TValue> innerQueryable, IQueryableQueryFactory factory, IEnumerable<IQueryInterceptor> interceptors=null) {
            InnerQueryable = innerQueryable;
            _factory = factory;
            InterceptorList = interceptors == null ? new List<IQueryInterceptor>() : new List<IQueryInterceptor>(interceptors);
        }

        /// <inheritdoc />
        public IQueryExecutor<TValue> QueryExecutor => _queryExecutor ?? (_queryExecutor = CreateExecutor());
        /// <summary>
        /// The internal interceptor list
        /// </summary>
        protected readonly List<IQueryInterceptor> InterceptorList;

        /// <inheritdoc />
        public IEnumerable<IQueryInterceptor> Interceptors => InterceptorList;

        /// <summary>
        /// The internal <see cref="IQueryable{T}"/>
        /// </summary>
        public IQueryable<TValue> InnerQueryable { get; }

        #region Where
        /// <inheritdoc />
        public IQuery<TValue> Where(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(InnerQueryable.Where(predicate));
        }

        /// <inheritdoc />
        public IQuery<TValue> Where(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(InnerQueryable.Where(predicate));
        }

        #endregion

        #region Select

        /// <inheritdoc />
        public IQuery<TResult> Select<TResult>(Expression<Func<TValue, TResult>> selector) {
            return CreateQuery(InnerQueryable.Select(selector));
        }

        /// <inheritdoc />
        public IQuery<TResult> Select<TResult>(Expression<Func<TValue, int, TResult>> selector) {
            return CreateQuery(InnerQueryable.Select(selector));
        }

        /// <inheritdoc />
        public IQuery<TResult> SelectMany<TResult>(Expression<Func<TValue, IEnumerable<TResult>>> selector) {
            return CreateQuery(InnerQueryable.SelectMany(selector));
        }

        /// <inheritdoc />
        public IQuery<TResult> SelectMany<TResult>(Expression<Func<TValue, int, IEnumerable<TResult>>> selector) {
            return CreateQuery(InnerQueryable.SelectMany(selector));
        }

        /// <inheritdoc />
        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<TValue, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TValue, TCollection, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        /// <inheritdoc />
        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<TValue, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TValue, TCollection, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        #endregion

        #region Join

        /// <inheritdoc />
        public IQuery<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TValue, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TValue, TInner, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        /// <inheritdoc />
        public IQuery<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TValue, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TValue, IEnumerable<TInner>, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        #endregion

        #region OrderBy

        /// <inheritdoc />
        public IOrderedQuery<TValue> OrderBy<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(InnerQueryable.OrderBy(keySelector));
        }

        /// <inheritdoc />
        public IOrderedQuery<TValue> OrderByDescending<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(InnerQueryable.OrderByDescending(keySelector));
        }

        #endregion

        #region Take

        /// <inheritdoc />
        public IQuery<TValue> Take(int count) {
            return CreateQuery(InnerQueryable.Take(count));
        }

        /// <inheritdoc />
        public IQuery<TValue> TakeWhile(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(InnerQueryable.TakeWhile(predicate));
        }

        /// <inheritdoc />
        public IQuery<TValue> TakeWhile(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(InnerQueryable.TakeWhile(predicate));
        }

        #endregion

        #region Skip

        /// <inheritdoc />
        public IQuery<TValue> Skip(int count) {
            return CreateQuery(InnerQueryable.Skip(count));
        }

        /// <inheritdoc />
        public IQuery<TValue> SkipWhile(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(InnerQueryable.SkipWhile(predicate));
        }

        /// <inheritdoc />
        public IQuery<TValue> SkipWhile(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(InnerQueryable.SkipWhile(predicate));
        }

        #endregion

        #region GroupBy

        /// <inheritdoc />
        public IQuery<IGrouping<TKey, TValue>> GroupBy<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(InnerQueryable.GroupBy(keySelector));
        }

        /// <inheritdoc />
        public IQuery<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TValue, TElement>> elementSelector) {
            return CreateQuery(InnerQueryable.GroupBy(keySelector, elementSelector));
        }

        /// <inheritdoc />
        public IQuery<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TValue, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.GroupBy(keySelector, elementSelector, resultSelector));
        }

        /// <inheritdoc />
        public IQuery<TResult> GroupBy<TKey, TResult>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TValue>, TResult>> resultSelector) {
            return CreateQuery(InnerQueryable.GroupBy(keySelector, resultSelector));
        }

        #endregion

        #region Misc

        /// <inheritdoc />
        public IQuery<TValue> Distinct() {
            return CreateQuery(InnerQueryable.Distinct());
        }

        /// <inheritdoc />
        public IQuery<TValue> Concat(IEnumerable<TValue> source2) {
            return CreateQuery(InnerQueryable.Concat(source2));
        }

        /// <inheritdoc />
        public IQuery<TValue> Union(IEnumerable<TValue> source2) {
            return CreateQuery(InnerQueryable.Union(source2));
        }

        /// <inheritdoc />
        public IQuery<TValue> Intersect(IEnumerable<TValue> source2) {
            return CreateQuery(InnerQueryable.Intersect(source2));
        }

        /// <inheritdoc />
        public IQuery<TValue> Except(IEnumerable<TValue> source2) {
            return CreateQuery(InnerQueryable.Except(source2));
        }

        /// <inheritdoc />
        public IQuery<TValue> Reverse() {
            return CreateQuery(InnerQueryable.Reverse());
        }

        /// <inheritdoc />
        public IQuery<TValue> InterceptQuery(IQueryInterceptor interceptor) {
            InterceptorList.Add(interceptor);
            return this;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create a new <see cref="IQuery{T}"/> using the internal <see cref="IQueryableQueryFactory"/>
        /// </summary>
        protected IQuery<T> CreateQuery<T>(IQueryable<T> queryable) {
            return _factory.CreateQuery(queryable, InterceptorList);
        }

        /// <summary>
        /// Create a new <see cref="IOrderedQuery{T}"/> using the internal <see cref="IQueryableQueryFactory"/>
        /// </summary>
        protected IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable) {
            return _factory.CreateQuery(queryable, InterceptorList);
        }

        /// <summary>
        /// Create a new <see cref="IQueryExecutor{T}"/> using the internal <see cref="IQueryableQueryFactory"/>
        /// </summary>
        protected IQueryExecutor<TValue> CreateExecutor() {
            return _factory.CreateExecutor(this);
        }
        #endregion
    }
}
