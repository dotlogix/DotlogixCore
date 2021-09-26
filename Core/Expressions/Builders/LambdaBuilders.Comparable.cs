// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Comparable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:02
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        
        #region Comparable
        
        public static LambdaBuilder<bool> Equal<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.Equal(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> NotEqual<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.NotEqual(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> IsGreaterThan<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.GreaterThan(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> IsGreaterThanOrEqual<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.GreaterThanOrEqual(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> IsLessThan<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.LessThan(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> IsLessThanOrEqual<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.LessThanOrEqual(instance.Body, other.Body);
        }

        public static LambdaBuilder<bool> LaysBetween<T>(this LambdaBuilder<T> instance, Optional<T> min = default, Optional<T> max = default) {
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
                return instance.Equal(min.Value);
            }
            return instance.IsGreaterThanOrEqual(min.Value).AndAlso(instance.IsLessThanOrEqual(max.Value));

        }
        #endregion
        
    }
}
