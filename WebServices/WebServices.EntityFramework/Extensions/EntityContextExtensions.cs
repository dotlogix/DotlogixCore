// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  EntityContextExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 31.07.2022 04:20
// LastEdited:  31.07.2022 04:20
// ==================================================

using System;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class EntityContextExtensions {
    public static IQueryable<T> Query<T>(this IEntityContext entityContext) where T : class {
        return entityContext.Set<T>().Query();
    }

    public static IQueryable<T> Query<T>(this IEntityContext entityContext, string sql, params object[] parameters) where T : class {
        return entityContext.Set<T>().Query(sql, parameters);
    }

    public static IQueryable<T> Query<T>(this IEntityContext entityContext, FormattableString sql) where T : class {
        return entityContext.Set<T>().Query(sql);
    }
    
}