using System.Linq.Expressions;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Expressions;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.EntityFramework.Expressions;
using DotLogix.WebServices.EntityFramework.Expressions.Rewriters;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Context {
    public class NpgSqlEntityContext<TDbContext> : WebServiceEntityContext<TDbContext> where TDbContext : DbContext {
        public NpgSqlEntityContext(IDbContextFactory<TDbContext> dbContextFactory, ILogSourceProvider logSourceProvider = null, ICacheProvider cacheProvider = null)
            : base(dbContextFactory, cacheProvider, logSourceProvider) {
        }

        protected override ExpressionVisitor CreateQueryRewriter() {
            var rewriter = new RewritingExpressionVisitor();
            rewriter.Use(new NpgSqlSearchTermRewriter());
            rewriter.Use(new RangeTermRewriter());
            rewriter.Use(new ManyTermRewriter());
            rewriter.Use(new ExpressionEvaluateRewriter());
            return rewriter;
        }
    }
}