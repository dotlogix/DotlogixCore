using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public sealed class RecursiveSqlCteQueryBuilder<TResult> : SqlCteBuilder<TResult, IRecursiveSqlCteQueryBuilder<TResult>>, IRecursiveSqlCteQueryBuilder<TResult>
    where TResult : class {
    private IQueryable<TResult> _query;

    public RecursiveSqlCteQueryBuilder(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) : base(entityContext, references) {
    }

    public IRecursiveSqlCteQueryBuilder<TResult> UseQuery(Func<IRecursiveSqlCteContext<TResult>, IQueryable<TResult>> queryFunc) {
        _query = queryFunc.Invoke(new RecursiveSqlCteContext<TResult>(EntityContext, References, RecursiveReference));
        return this;
    }

    protected override IQueryable<TResult> BuildQuery() => _query;
}