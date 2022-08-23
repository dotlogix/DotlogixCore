// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  Lambdas.EF.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 26.05.2022 17:51
// LastEdited:  26.05.2022 17:51
// ==================================================

using System;
using System.Linq.Expressions;
using DotLogix.Core.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class LambdaExtensions {
    public static Lambda<TValue> Property<TEntity, TValue>(this Lambda<TEntity> entity, IReadOnlyProperty property)
        where TEntity : class {
        if(property.PropertyInfo is { } propertyInfo) {
            return entity.Property<TValue>(propertyInfo);
        }

        if(property.FieldInfo is { } fieldInfo) {
            return entity.Field<TValue>(fieldInfo);
        }

        return entity.CallStatic(EF.Property<TValue>, Lambdas.Constant(property.Name));
    }
    
    public static Expression<Func<TEntity, TValue>> AsExpression<TEntity, TValue>(this IReadOnlyProperty property)
        where TEntity : class {
        var (entityLambda, entityParameter) = Lambdas.Parameter<TEntity>();
        return entityLambda
           .Property<TEntity, TValue>(property)
           .ToLambda<TEntity, TValue>(entityParameter);
    }
}