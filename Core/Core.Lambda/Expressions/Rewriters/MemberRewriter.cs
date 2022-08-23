using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions.Rewriters {
    public class MemberRewriter : IMemberRewriter {
        private readonly Func<Expression, MemberInfo, Expression> _rewrite;

        public MemberRewriter(Func<Expression, MemberInfo, Expression> rewrite) {
            _rewrite = rewrite;
        }

        public Expression Rewrite(Expression instance, MemberInfo member) {
            return _rewrite.Invoke(instance, member);
        }
    }
}