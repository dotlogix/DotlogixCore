using System.Linq.Expressions;
using DotLogix.Core.Caching;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Expressions;
using DotLogix.Infrastructure.EntityFramework;
using DotLogix.Infrastructure.EntityFramework.Hooks;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Expressions.Rewriters;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Context {
    public class WebServiceEntityContext<TDbContext> : EfEntityContext<TDbContext>, IWebServiceEntityContext where TDbContext : DbContext {
        private ExpressionVisitor _expressionRewriter;
        protected ExpressionVisitor ExpressionRewriter => _expressionRewriter ??= CreateQueryRewriter();
        protected ICacheProvider CacheProvider { get; }
        
        public ICache<object, object> EntityCache { get; }
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public WebServiceEntityContext(IDbContextFactory<TDbContext> dbContextFactory, ICacheProvider cacheProvider, ILogSourceProvider logSourceProvider)
            : base(dbContextFactory, logSourceProvider) {
            CacheProvider = cacheProvider;
            EntityCache = CacheProvider.GetOrCreateEntityCache();
        }

        protected virtual ExpressionVisitor CreateQueryRewriter() {
            var rewriter = new RewritingExpressionVisitor();
            rewriter.Use(new SearchTermRewriter());
            rewriter.Use(new RangeTermRewriter());
            rewriter.Use(new ManyTermRewriter());
            rewriter.Use(new ExpressionEvaluateRewriter());
            return rewriter;
        }

        protected override void OnQueryCompile(object sender, QueryCompileEventArgs e) {
            e.SetExpression(ExpressionRewriter.Visit(e.Expression));
            base.OnQueryCompile(sender, e);
        }
    }
}