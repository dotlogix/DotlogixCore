using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class ExpressionEvaluateRewriter : IMethodCallRewriter {
        public Expression Rewrite(Expression expression) {
            return expression is MethodCallExpression mce ? Rewrite(mce) : expression;
        }

        public MethodInfo MatchesMethod { get; } = typeof(ExpressionExtensions).GetMethods().First(m => m.Name == "Evaluate" && m.IsGenericMethod);

        public Expression Rewrite(MethodCallExpression expression) {
            if(!(expression.Object is LambdaExpression lambda)) {
                return expression;
            }

            var arguments = ((NewArrayExpression)expression.Arguments[0]).Expressions;
            return LambdaBuilders.Inline(lambda, arguments.Select(LambdaBuilders.From));
        }

        public bool CanRewrite(Expression expression) {
            return expression is MethodCallExpression ex && ex.Method.IsGenericMethod && ex.Method.GetGenericMethodDefinition() == MatchesMethod;
        }
    }
}
