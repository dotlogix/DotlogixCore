using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class RangeTermRewriter : IMethodCallRewriter {
        private readonly MethodInfo _rewriteInternalMethod = typeof(ManyTermRewriter).GetMethod(nameof(RewriteInternal));
        
        public Expression Rewrite(Expression expression) {
            return expression is MethodCallExpression mce ? Rewrite(mce) : expression;
        }

        public MethodInfo MatchesMethod { get; } = TermExtensions.GetTermMethodInfo(typeof(RangeTerm<>));

        public Expression Rewrite(MethodCallExpression expression) {
            var type = expression.Object!.Type.GetGenericArguments()[0];
            return (Expression)_rewriteInternalMethod.MakeGenericMethod(type).Invoke(this, expression.CreateArray<object>());
        }

        public Expression RewriteInternal<T>(MethodCallExpression expression) {            
            var term = expression.Object.Evaluate<RangeTerm<T>>();
            return LambdaBuilders.LaysBetween(expression.Arguments[0], term.Min, term.Max);
        }

        public bool CanRewrite(Expression expression) {
            return expression is MethodCallExpression ex && ex.Method.IsGenericMethod && ex.Method.GetGenericMethodDefinition() == MatchesMethod;
        }
    }
}