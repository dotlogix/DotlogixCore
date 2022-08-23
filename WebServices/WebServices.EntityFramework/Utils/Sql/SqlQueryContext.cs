using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class SqlQueryContext : ISqlQueryContext {
    public IEntityContext EntityContext { get; }
    public IReadOnlyDictionary<string, ISqlCte> References { get; }

    public SqlQueryContext(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) {
        EntityContext = entityContext;
        References = references;
    }
}