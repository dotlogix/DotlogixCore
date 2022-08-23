using System.Collections.Generic;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public class SqlCteContext : SqlQueryContext, ISqlCteContext {
    public SqlCteContext(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) : base(entityContext, references) {
    }
}