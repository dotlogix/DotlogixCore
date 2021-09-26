// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ExpressionExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq.Expressions;
using System.Reflection;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="Expression"/>
    /// </summary>
    public static class ExpressionExtensions {
        /// <summary>
        ///     Compiles and invokes the expression
        /// </summary>
        public static T Evaluate<T>(this Expression expression, params object[] parameters) {
            return Evaluate(expression, parameters) is T t ? t : default;
        }

        /// <summary>
        ///     Compiles and invokes the expression
        /// </summary>
        public static object Evaluate(this Expression expression, params object[] parameters) {
            return (expression as LambdaExpression)?.Compile().DynamicInvoke(parameters);
        }
        
        /// <summary>
        ///     Gets the target member of a expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetTargetMember<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member;
        }

        /// <summary>
        ///     Gets the target field of a expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FieldInfo GetTargetField<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member as FieldInfo;
        }

        /// <summary>
        ///     Gets the target property of a expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetTargetProperty<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member as PropertyInfo;
        }
    }
}
