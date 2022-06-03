using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class EntityQueryPreProcessorFactory : RelationalQueryTranslationPreprocessorFactory {
    private readonly IEntityQueryRewriter _rewriter;

    public EntityQueryPreProcessorFactory(
        QueryTranslationPreprocessorDependencies dependencies,
        RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
        IEntityQueryRewriter rewriter
    ) : base(dependencies, relationalDependencies) {
        _rewriter = rewriter;
    }

    public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext) {
        return new EntityQueryPreProcessor(Dependencies, queryCompilationContext, _rewriter);
    }
}