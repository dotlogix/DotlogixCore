using System.Collections.Generic;
using System.Linq.Expressions;
using DotLogix.Core.Expressions.Rewriters;

namespace DotLogix.WebServices.EntityFramework.Database {
    public class EntityQueryRewriter : IEntityQueryRewriter {
        private readonly RewritingExpressionVisitor _rewriter;
        private readonly int _totalCount;

        public EntityQueryRewriter(
            IEnumerable<IMethodCallRewriter> methodRewriters, 
            IEnumerable<IMemberRewriter> memberRewriters, 
            IEnumerable<IExpressionRewriter> rewriters
        ) {
            _rewriter = new RewritingExpressionVisitor();
            _rewriter.AddRange(methodRewriters);
            _rewriter.AddRange(memberRewriters);
            _rewriter.AddRange(rewriters);
            _totalCount = _rewriter.Rewriters.Count + _rewriter.MemberRewriters.Count + _rewriter.MethodRewriters.Count;
        }

        public EntityQueryRewriter(RewritingExpressionVisitor rewriter) {
            _rewriter = rewriter;
        }

        public Expression Rewrite(Expression expression) {
            return _totalCount > 0 ? _rewriter.Visit(expression) : expression;
        }
    }
}