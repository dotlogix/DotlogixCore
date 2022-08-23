// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  QueryHierarchyExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 31.07.2022 04:20
// LastEdited:  31.07.2022 04:20
// ==================================================

using System;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.WebServices.EntityFramework.Utils.Sql;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class QueryHierarchyExtensions {
    public static ISqlQueryBuilder UseAncestorCte<T, TKey>(
        this ISqlQueryBuilder builder,
        string referenceName,
        Expression<Func<T, bool>> descendantFilter,
        Expression<Func<T, TKey>> ancestorKeySelector,
        Expression<Func<T, TKey>> descendantKeySelector
    ) where T : class {
        return builder.UseHierarchyCte<T, TKey>(cte => cte
           .UseName(referenceName)
           .UseKey(ancestorKeySelector)
           .UseNextKey(descendantKeySelector)
           .UseInitialFilter(descendantFilter)
        );
    }
    
    public static ISqlQueryBuilder UseDescendantCte<T, TKey>(
        this ISqlQueryBuilder builder,
        string referenceName,
        Expression<Func<T, bool>> ancestorFilter,
        Expression<Func<T, TKey>> ancestorKeySelector,
        Expression<Func<T, TKey>> descendantKeySelector
    ) where T : class {
        return builder.UseHierarchyCte<T, TKey>(cte => cte
           .UseName(referenceName)
           .UseKey(descendantKeySelector)
           .UseNextKey(ancestorKeySelector)
           .UseInitialFilter(ancestorFilter)
        );
    }
    
    public static ISqlQueryBuilder UseParentCte<T, TKey>(
        this ISqlQueryBuilder builder,
        string referenceName,
        Expression<Func<T, bool>> childFilter,
        Expression<Func<T, TKey>> ancestorKeySelector,
        Expression<Func<T, TKey>> descendantKeySelector
    ) where T : class {
        return builder.UseCte<T>(cte => cte
           .UseName(referenceName)
           .UseQuery(context => {
                var query = context.Query<T>();
                var current = childFilter is not null ? query.Where(childFilter) : query;
                return query.WhereRelated(descendantKeySelector, current, ancestorKeySelector);
            })
        );
    }
    
    public static ISqlQueryBuilder UseChildrenCte<T, TKey>(
        this ISqlQueryBuilder builder,
        string referenceName,
        Expression<Func<T, bool>> parentFilter,
        Expression<Func<T, TKey>> ancestorKeySelector,
        Expression<Func<T, TKey>> descendantKeySelector
    ) where T : class {
        return builder.UseCte<T>(cte => cte
           .UseName(referenceName)
           .UseQuery(context => {
                var query = context.Query<T>();
                var current = parentFilter is not null ? query.Where(parentFilter) : query;
                return query.WhereRelated(ancestorKeySelector, current, descendantKeySelector);
            })
        );
    }
}