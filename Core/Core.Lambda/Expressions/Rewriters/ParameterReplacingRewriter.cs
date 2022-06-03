using System;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions.Rewriters;

public class ParameterReplacingRewriter : IParameterRewriter {
    private readonly ParameterExpression[] _source;
    private readonly Expression[] _target;

    public ParameterReplacingRewriter(ParameterExpression[] source, Expression[] target) {
        _source = source;
        _target = target;
    }
    
    public ParameterReplacingRewriter(ParameterExpression source, Expression target) {
        _source = new []{source};
        _target = new []{target};
    }

    public Expression Rewrite(ParameterExpression expression) {
        var idx = Array.IndexOf(_source, expression);
        return idx >= 0 ? _target[idx] : default;
    }
}