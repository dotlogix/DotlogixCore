using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class ManyTermRewriter : IMethodCallRewriter {
        private readonly MethodInfo _rewriteInternalMethod = typeof(ManyTermRewriter).GetMethod(nameof(RewriteInternal));
        
        public Expression Rewrite(Expression expression) {
            return expression is MethodCallExpression mce ? Rewrite(mce) : expression;
        }

        public MethodInfo MatchesMethod { get; } = TermExtensions.GetTermMethodInfo(typeof(ManyTerm<>));

        public Expression Rewrite(MethodCallExpression expression) {
            var type = expression.Object!.Type.GetGenericArguments()[0];
            return (Expression)_rewriteInternalMethod.MakeGenericMethod(type).Invoke(this, new []{expression});
        }

        public Expression RewriteInternal<T>(MethodCallExpression expression) {            
            var term = expression.Object.Evaluate<ManyTerm<T>>();
            switch(term.Count) {
                case 0:
                    return LambdaBuilders.True;
                case 1:
                    return LambdaBuilders.From<T>(expression.Arguments[0]).Equal(term.Values[0]);
                default:
                    return LambdaBuilders.FromValue<IEnumerable<T>>(term.Values).Contains(expression.Arguments[0]);
            }
        }

        public bool CanRewrite(Expression expression) {
            return expression is MethodCallExpression ex && ex.Method.IsGenericMethod && ex.Method.GetGenericMethodDefinition() == MatchesMethod;
        }
    }
}