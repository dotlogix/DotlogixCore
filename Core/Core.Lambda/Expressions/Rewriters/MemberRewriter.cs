using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions.Rewriters;

public class MemberRewriter : IMemberRewriter {
    private readonly Func<Expression, MemberInfo, Type, Expression> _rewrite;

    public MemberRewriter(Func<Expression, MemberInfo, Type, Expression> rewrite) {
        _rewrite = rewrite;
    }

    public Expression Rewrite(Expression instance, MemberInfo member, Type type) {
        return _rewrite.Invoke(instance, member, type);
    }
}