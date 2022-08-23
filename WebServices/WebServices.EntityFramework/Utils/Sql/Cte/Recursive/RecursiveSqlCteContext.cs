using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public class RecursiveSqlCteContext<TCurrent> : SqlCteContext, IRecursiveSqlCteContext<TCurrent> {
    private readonly IQueryable<TCurrent> _current;

    public RecursiveSqlCteContext(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references, IQueryable<TCurrent> current) : base(entityContext, references) {
        _current = current;
    }

    public IQueryable<TCurrent> Current() => _current;
}