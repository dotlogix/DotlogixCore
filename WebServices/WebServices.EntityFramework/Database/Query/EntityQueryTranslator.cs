using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Expressions.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database;

public class EntityQueryTranslator : IEntityQueryTranslator {
    public IReadOnlyCollection<IMethodCallSqlTranslator> MethodTranslators { get; }
    public IReadOnlyCollection<IMemberSqlTranslator> MemberTranslators { get; }

    public EntityQueryTranslator(
        IEnumerable<IMethodCallSqlTranslator> methodRewriters, 
        IEnumerable<IMemberSqlTranslator> memberRewriters
    ) {
        MethodTranslators = methodRewriters.AsReadOnlyCollection();
        MemberTranslators = memberRewriters.AsReadOnlyCollection();
    }
}