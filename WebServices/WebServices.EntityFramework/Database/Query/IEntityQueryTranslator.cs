using System.Collections.Generic;
using DotLogix.WebServices.EntityFramework.Expressions.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database;

public interface IEntityQueryTranslator {
    public IReadOnlyCollection<IMethodCallSqlTranslator> MethodTranslators { get; }
    public IReadOnlyCollection<IMemberSqlTranslator> MemberTranslators { get; }
}