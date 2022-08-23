using System;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions.Rewriters {
    public class ParameterRewriter : IParameterRewriter {
        private readonly Func<ParameterExpression, Expression> _rewrite;

        public ParameterRewriter(Func<ParameterExpression, Expression> rewrite) {
            _rewrite = rewrite;
        }

        public Expression Rewrite(ParameterExpression expression) {
            return _rewrite.Invoke(expression);
        }
    }
}