// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ExpressionExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Linq.Expressions;
using System.Reflection;
#endregion

namespace DotLogix.Core.Extensions {
    public static class ExpressionExtensions {
        public static MemberInfo GetTargetMember<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member;
        }

        public static FieldInfo GetTargetField<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member as FieldInfo;
        }

        public static PropertyInfo GetTargetProperty<TSource, TTarget>(this Expression<Func<TSource, TTarget>> expression) {
            var memberExpression = expression?.Body as MemberExpression;
            return memberExpression?.Member as PropertyInfo;
        }
    }
}
