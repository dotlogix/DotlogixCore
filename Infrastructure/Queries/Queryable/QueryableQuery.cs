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
    public class QueryableQuery<TValue> : IQuery<TValue> {
        private readonly IQueryableQueryFactory _factory;
        private readonly IQueryable<TValue> _innerQueryable;
        private IQueryExecutor<TValue> _queryExecutor;

        public QueryableQuery(IQueryable<TValue> innerQueryable, IQueryableQueryFactory factory) {
            _innerQueryable = innerQueryable;
            _factory = factory;
        }

        public IQueryExecutor<TValue> QueryExecutor => _queryExecutor ?? (_queryExecutor = CreateExecutor());

        #region Where

        public IQuery<TValue> Where(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(_innerQueryable.Where(predicate));
        }

        public IQuery<TValue> Where(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(_innerQueryable.Where(predicate));
        }

        #endregion

        #region Select

        public IQuery<TResult> Select<TResult>(Expression<Func<TValue, TResult>> selector) {
            return CreateQuery(_innerQueryable.Select(selector));
        }

        public IQuery<TResult> Select<TResult>(Expression<Func<TValue, int, TResult>> selector) {
            return CreateQuery(_innerQueryable.Select(selector));
        }

        public IQuery<TResult> SelectMany<TResult>(Expression<Func<TValue, IEnumerable<TResult>>> selector) {
            return CreateQuery(_innerQueryable.SelectMany(selector));
        }

        public IQuery<TResult> SelectMany<TResult>(Expression<Func<TValue, int, IEnumerable<TResult>>> selector) {
            return CreateQuery(_innerQueryable.SelectMany(selector));
        }

        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<TValue, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TValue, TCollection, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        public IQuery<TResult> SelectMany<TCollection, TResult>(Expression<Func<TValue, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TValue, TCollection, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.SelectMany(collectionSelector, resultSelector));
        }

        #endregion

        #region Join

        public IQuery<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TValue, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TValue, TInner, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        public IQuery<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TValue, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TValue, IEnumerable<TInner>, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
        }

        #endregion

        #region OrderBy

        public IOrderedQuery<TValue> OrderBy<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.OrderBy(keySelector));
        }

        public IOrderedQuery<TValue> OrderByDescending<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.OrderByDescending(keySelector));
        }

        #endregion

        #region Take

        public IQuery<TValue> Take(int count) {
            return CreateQuery(_innerQueryable.Take(count));
        }

        public IQuery<TValue> TakeWhile(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(_innerQueryable.TakeWhile(predicate));
        }

        public IQuery<TValue> TakeWhile(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(_innerQueryable.TakeWhile(predicate));
        }

        #endregion

        #region Skip

        public IQuery<TValue> Skip(int count) {
            return CreateQuery(_innerQueryable.Skip(count));
        }

        public IQuery<TValue> SkipWhile(Expression<Func<TValue, bool>> predicate) {
            return CreateQuery(_innerQueryable.SkipWhile(predicate));
        }

        public IQuery<TValue> SkipWhile(Expression<Func<TValue, int, bool>> predicate) {
            return CreateQuery(_innerQueryable.SkipWhile(predicate));
        }

        #endregion

        #region GroupBy

        public IQuery<IGrouping<TKey, TValue>> GroupBy<TKey>(Expression<Func<TValue, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.GroupBy(keySelector));
        }

        public IQuery<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TValue, TElement>> elementSelector) {
            return CreateQuery(_innerQueryable.GroupBy(keySelector, elementSelector));
        }

        public IQuery<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TValue, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.GroupBy(keySelector, elementSelector, resultSelector));
        }

        public IQuery<TResult> GroupBy<TKey, TResult>(Expression<Func<TValue, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TValue>, TResult>> resultSelector) {
            return CreateQuery(_innerQueryable.GroupBy(keySelector, resultSelector));
        }

        #endregion

        #region Misc

        public IQuery<TValue> Distinct() {
            return CreateQuery(_innerQueryable.Distinct());
        }

        public IQuery<TValue> Concat(IEnumerable<TValue> source2) {
            return CreateQuery(_innerQueryable.Concat(source2));
        }

        public IQuery<TValue> Union(IEnumerable<TValue> source2) {
            return CreateQuery(_innerQueryable.Union(source2));
        }

        public IQuery<TValue> Intersect(IEnumerable<TValue> source2) {
            return CreateQuery(_innerQueryable.Intersect(source2));
        }

        public IQuery<TValue> Except(IEnumerable<TValue> source2) {
            return CreateQuery(_innerQueryable.Except(source2));
        }

        public IQuery<TValue> Reverse() {
            return CreateQuery(_innerQueryable.Reverse());
        }

        #endregion

        #region Create
        protected IQuery<T> CreateQuery<T>(IQueryable<T> queryable) {
            return _factory.CreateQuery(queryable);
        }

        protected IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable) {
            return _factory.CreateQuery(queryable);
        }

        protected IQueryExecutor<TValue> CreateExecutor() {
            return _factory.CreateExecutor(_innerQueryable);
        }
        #endregion
    }
}
