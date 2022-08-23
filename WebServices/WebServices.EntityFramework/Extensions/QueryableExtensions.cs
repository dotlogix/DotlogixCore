// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  QueryableExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 10.12.2021 00:11
// LastEdited:  10.12.2021 00:11
// ==================================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class QueryableExtensions {
    private static readonly MethodInfo OrderByMethod = ((Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.OrderBy).Method.GetGenericMethodDefinition();
    private static readonly MethodInfo OrderByDescendingMethod = ((Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.OrderByDescending).Method.GetGenericMethodDefinition();
    private static readonly MethodInfo ThenByMethod = ((Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.ThenBy).Method.GetGenericMethodDefinition();
    private static readonly MethodInfo ThenByDescendingMethod = ((Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.ThenByDescending).Method.GetGenericMethodDefinition();

    public static IQueryable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult> (
        this IQueryable<TLeft> leftItems,
        IEnumerable<TRight> rightItems,
        Expression<Func<TLeft, TKey>> leftKeySelector,
        Expression<Func<TRight, TKey>> rightKeySelector,
        Expression<Func<TLeft, TRight, TResult>> resultSelector) {
        var (groupResult, groupResultParameter) = Lambdas.Parameter<GroupJoinResult<TLeft, TRight>>(); 
        var (right, rightParameter) = Lambdas.Parameter<TRight>();

        var resultSelectorExpression = groupResult
           .Property<TLeft>(nameof(GroupJoinResult<TLeft, TRight>.Outer))
           .Invoke(resultSelector, right)
           .ToLambda<GroupJoinResult<TLeft, TRight>, TRight, TResult>(groupResultParameter, rightParameter);

        var result = leftItems
           .GroupJoin(rightItems, leftKeySelector, rightKeySelector, (left, rightCollection) => new GroupJoinResult<TLeft, TRight> { Outer = left, Inner = rightCollection })
           .SelectMany(joined => joined.Inner.DefaultIfEmpty(), resultSelectorExpression);

        return result;
    }

    public static IQueryable<TResult> RightOuterJoin<TLeft, TRight, TKey, TResult> (
        this IQueryable<TLeft> leftItems,
        IEnumerable<TRight> rightItems,
        Expression<Func<TLeft, TKey>> leftKeySelector,
        Expression<Func<TRight, TKey>> rightKeySelector,
        Expression<Func<TLeft, TRight, TResult>> resultSelector)
    {
        var groupResultParameter = Expression.Parameter(typeof(GroupJoinResult<TRight, TLeft>));
        var leftParameter = Expression.Parameter(typeof(TLeft));

        var rightProperty = Lambdas
           .From<GroupJoinResult<TRight, TLeft>>(groupResultParameter)
           .Property<TRight>(nameof(GroupJoinResult<TRight, TLeft>.Outer));
            
        var resultSelectorExpression = Lambdas.Invoke(resultSelector, leftParameter, rightProperty)
           .ToLambda<GroupJoinResult<TRight, TLeft>, TLeft, TResult>(groupResultParameter, leftParameter);

        var result = rightItems.AsQueryable()
           .GroupJoin(leftItems, rightKeySelector, leftKeySelector, (right, leftCollection) => new GroupJoinResult<TRight, TLeft> { Outer = right, Inner = leftCollection })
           .SelectMany(joined => joined.Inner.DefaultIfEmpty(), resultSelectorExpression);

        return result;
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static IQueryable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult> (
        this IQueryable<TLeft> leftItems,
        IEnumerable<TRight> rightItems,
        Expression<Func<TLeft, TKey>> leftKeySelector,
        Expression<Func<TRight, TKey>> rightKeySelector,
        Expression<Func<TLeft, TRight, TResult>> resultSelector)
    {
        return leftItems
           .LeftOuterJoin(rightItems, leftKeySelector, rightKeySelector, resultSelector)
           .Union(leftItems.RightOuterJoin(rightItems, leftKeySelector, rightKeySelector, resultSelector));
    }

    public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) {
        return await query.ToListAsync(cancellationToken);
    }
        
    public static async Task<PaginationResult<T>> ToPagedAsync<T>(this IQueryable<T> query, PaginationTerm paginationTerm, CancellationToken cancellationToken = default) {
        List<T> entities;
        int totalCount;

        var page = paginationTerm.Page.GetValueOrDefault(-1);
        var pageSize = page > 0 ? paginationTerm.PageSize : -1;
        var orderBy = paginationTerm.OrderBy;
        if(page > 0) {
            totalCount = await query.CountAsync(cancellationToken);

            query = OrderBy(query, orderBy);
            query = ApplyPage(query, page, pageSize);
            entities = await query.ToListAsync(cancellationToken);
        } else {
            query = OrderBy(query, orderBy);
            entities = await query.ToListAsync(cancellationToken);
            totalCount = entities.Count;
        }

        return new PaginationResult<T> {
            Values = entities,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy
        };
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, ManyTerm<OrderingKey> orderKeys = null) {
        if(orderKeys == null || orderKeys.Count == 0) {
            if(typeof(T).IsAssignableTo<IOrdered>()) {
                orderKeys = new OrderingKey { Name = nameof(IOrdered.Order), Direction = SortDirection.Ascending };
            } else {
                throw new ArgumentException("At least one ordering key is required", nameof(orderKeys));
            }
        }
            
        Lambda builder = Lambdas.FromQueryable(query);
        for(var i = 0; i < orderKeys.Count; i++) {
            var orderingKey = orderKeys[i];
                
            var (entity, entityParameter) = Lambdas.Parameter<T>();
            var property = entity.PropertyOrField(orderingKey.Name);
            Lambda lambda = Expression.Lambda(property, entityParameter);
                
            MethodInfo orderMethod;
            if(i == 0) {
                orderMethod = orderingKey.Direction == SortDirection.Ascending ? OrderByMethod : OrderByDescendingMethod;
            } else {
                orderMethod = orderingKey.Direction == SortDirection.Ascending ? ThenByMethod : ThenByDescendingMethod;
            }

            orderMethod = orderMethod.MakeGenericMethod(property.Type);
            builder = builder.Call<IOrderedQueryable<T>>(orderMethod, lambda);
        }
        return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(builder.Body);
    }

    private static IQueryable<T> ApplyPage<T>(IQueryable<T> query, int page, int pageSize) {
        return query.Skip(pageSize * page).Take(pageSize);
    }

    private class GroupJoinResult<TOuter, TInner> {
        public TOuter Outer { get; set; }
        public IEnumerable<TInner> Inner { get; set; }
    }
}