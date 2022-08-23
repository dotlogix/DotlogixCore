// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  Lambdas.Comparable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:02
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions; 

public static partial class Lambdas {
        
    #region Comparable
        
    public static Lambda<bool> IsEqualTo<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.Equal(instance.Body, other.Body);
    }

    public static Lambda<bool> IsNotEqualTo<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.NotEqual(instance.Body, other.Body);
    }

    public static Lambda<bool> IsGreaterThan<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.GreaterThan(instance.Body, other.Body);
    }

    public static Lambda<bool> IsGreaterThanOrEqual<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.GreaterThanOrEqual(instance.Body, other.Body);
    }

    public static Lambda<bool> IsLessThan<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.LessThan(instance.Body, other.Body);
    }

    public static Lambda<bool> IsLessThanOrEqual<T>(this Lambda<T> instance, Lambda<T> other) {
        return Expression.LessThanOrEqual(instance.Body, other.Body);
    }

    public static Lambda<bool> LaysBetween<T>(this Lambda<T> instance, Optional<T> min = default, Optional<T> max = default) {
        if(min.IsUndefined && max.IsUndefined) {
            return True;
        }

        if(min.IsUndefined) {
            return instance.IsLessThanOrEqual(max.Value);
        }

        if(max.IsUndefined) {
            return instance.IsGreaterThanOrEqual(min.Value);
        }
                
        if(Equals(min.Value, max.Value)) {
            return instance.IsEqualTo(min.Value);
        }
        return instance.IsGreaterThanOrEqual(min.Value).AndAlso(instance.IsLessThanOrEqual(max.Value));

    }
    #endregion
        
}