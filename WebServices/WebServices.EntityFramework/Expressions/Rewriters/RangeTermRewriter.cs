using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Expressions.Rewriters;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class RangeTermRewriter : IMethodCallRewriter {
        private static readonly MethodInfo TargetMethodInfo = TermExtensions.GetTermMethodInfo(typeof(RangeTerm<>));
        private static readonly MethodInfo RewriteInternalMethodInfo = typeof(ManyTermRewriter)
           .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
           .First(m => m.Name == nameof(Rewrite) && m.IsGenericMethod);

        public Expression Rewrite(Expression _, MethodInfo method, IReadOnlyList<Expression> arguments) {
            if(method.IsGenericMethod == false || method.GetGenericMethodDefinition() != TargetMethodInfo)
                return default;

            var (termExpression, valueExpression) = arguments;
            var term = termExpression.Evaluate<object>();
            var type = termExpression.Type.GetGenericArguments()[0];
            return (Expression)RewriteInternalMethodInfo.MakeGenericMethod(type).CreateDynamicInvoke().Invoke(null, term, valueExpression);
        }

        private static Expression Rewrite<T>(RangeTerm<T> term, Expression value) {
            return Lambdas.LaysBetween(value, term.Min, term.Max);
        }
    }
}