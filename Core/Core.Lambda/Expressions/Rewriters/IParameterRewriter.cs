using System.Linq.Expressions;

namespace DotLogix.Core.Expressions.Rewriters {
    public interface IParameterRewriter
    {
        Expression Rewrite(ParameterExpression expression);
    }
}