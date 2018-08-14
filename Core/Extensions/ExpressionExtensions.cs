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
    public static class ExpressionExtensions {
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
