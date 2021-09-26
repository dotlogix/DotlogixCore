// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:02
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        #region Boolean
        public static LambdaBuilder<bool> AndAlso(this LambdaBuilder<bool> instance, LambdaBuilder<bool> other) {
            return Expression.AndAlso(instance.Body, other.Body);
        }
        
        public static LambdaBuilder<bool> OrElse(this LambdaBuilder<bool> instance, LambdaBuilder<bool> other) {
            return Expression.OrElse(instance.Body, other.Body);
        }
        #endregion
        
        #region Bitwise
        public static LambdaBuilder<T> And<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) where T : struct {
            return Expression.And(instance.Body, other.Body);
        }
        
        public static LambdaBuilder<T> Or<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) where T : struct {
            return Expression.Or(instance.Body, other.Body);
        }
        
        public static LambdaBuilder<T> Xor<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) where T : struct {
            return Expression.ExclusiveOr(instance.Body, other.Body);
        }
        
        public static LambdaBuilder<T> Not<T>(this LambdaBuilder<T> instance) where T : struct {
            return Expression.Not(instance.Body);
        }
        
        public static LambdaBuilder<T> LeftShift<T>(this LambdaBuilder<T> instance, LambdaBuilder<int> count) where T : struct {
            return Expression.LeftShift(instance.Body, count.Body);
        }
        
        public static LambdaBuilder<T> RightShift<T>(this LambdaBuilder<T> instance, LambdaBuilder<int> count) where T : struct {
            return Expression.RightShift(instance.Body, count.Body);
        }
        #endregion
    }
}
