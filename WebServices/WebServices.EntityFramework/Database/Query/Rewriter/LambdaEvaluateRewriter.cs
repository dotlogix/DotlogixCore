using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Expressions.Rewriters;

namespace DotLogix.WebServices.EntityFramework.Database; 

public sealed class LambdaEvaluateRewriter : IMethodCallRewriter {
    public Expression Rewrite(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments) {
        if(method.DeclaringType != typeof(Lambdas) || method.Name != nameof(Lambdas.Evaluate)) {
            return default;
        }
            
        LambdaExpression lambdaExpression;
        switch (arguments[0])
        {
            case LambdaExpression lambda:
                lambdaExpression = lambda;
                break;
            case UnaryExpression { Operand: LambdaExpression quotedLambda }:
                lambdaExpression = quotedLambda;
                break;
            default:
                if (arguments.Count == 1){
                    // Handling calls to Lambdas.Evaluate(this Expression)
                    return arguments[0];
                }
                return null; // Not supported
        }

        IReadOnlyCollection<Expression> parameterReplacements;

        // Handling generic calls to Lambdas.Evaluate<T1, ..., TN>(this LambdaExpression, T1 arg1, ...TN argN)
        if (arguments.Count != 2 || method.GetParameters()[1].ParameterType != typeof(object[]))
        {
            parameterReplacements = arguments.Skip(1).ToArray();
        }
        else
        {
            // Handling calls to Lambdas.Evaluate(this LambdaExpression, params object[])
            switch (arguments[1])
            {
                case NewArrayExpression na:
                    // Array is created within the expression tree, so we can access the inner expressions directly
                    parameterReplacements = na.Expressions;
                    break;
                case ConstantExpression c:
                    // Array was created outside of the expression tree, so we need to convert all of them to constants
                    parameterReplacements = Array.ConvertAll<object, Expression>((object[]) c.Value!, Expression.Constant);
                    break;
                default:
                    return null; // Not supported
            }
        }

        var visitor = new RewritingExpressionVisitor();
        visitor.RewriteParameter(lambdaExpression.Parameters, parameterReplacements);
        var expression = visitor.Visit(lambdaExpression.Body);
        return expression;
    }
}