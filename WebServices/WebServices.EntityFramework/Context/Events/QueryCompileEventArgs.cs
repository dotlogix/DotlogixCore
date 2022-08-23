using System;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.EntityFramework.Context.Events; 

/// <summary>
/// A class to represent entity query event args
/// </summary>
public class QueryCompileEventArgs : QueryEventArgs {
    public Expression Expression { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="QueryCompileEventArgs"/>
    /// </summary>
    public QueryCompileEventArgs(Expression expression)
        : base(expression.Type) {
        Expression = expression;
    }
        
    public void SetExpression(Expression expression) {
        if(expression.Type != ResultType) {
            throw new ArgumentException($"Can not convert expression type {expression.Type.GetFriendlyGenericName()} to target return type {ResultType.GetFriendlyGenericName()}");
        }
        Expression = expression;
    }
}