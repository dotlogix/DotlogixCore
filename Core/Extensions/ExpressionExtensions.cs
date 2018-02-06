using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Extensions
{
    public static class ExpressionExtensions
    {
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
