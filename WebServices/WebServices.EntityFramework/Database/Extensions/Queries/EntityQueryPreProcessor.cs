using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public class EntityQueryPreProcessor : QueryTranslationPreprocessor {
    private readonly IEntityQueryRewriter _rewriter;

    public EntityQueryPreProcessor(
        QueryTranslationPreprocessorDependencies dependencies,
        QueryCompilationContext queryCompilationContext,
        IEntityQueryRewriter rewriter
    ) : base(dependencies, queryCompilationContext) {
        _rewriter = rewriter;
    }

    public override Expression Process(Expression query) {
        query = _rewriter.Rewrite(query);
        return base.Process(query);
    }
}