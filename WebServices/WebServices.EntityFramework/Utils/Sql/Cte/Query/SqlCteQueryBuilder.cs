using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public sealed class SqlCteQueryBuilder<TResult> : SqlCteBuilder<TResult, ISqlCteQueryBuilder<TResult>>, ISqlCteQueryBuilder<TResult>
    where TResult : class {
    private IQueryable<TResult> _query;

    public SqlCteQueryBuilder(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) : base(entityContext, references) {
    }

    public ISqlCteQueryBuilder<TResult> UseQuery(Func<ISqlCteContext, IQueryable<TResult>> queryFunc) {
        _query = queryFunc.Invoke(new SqlCteContext(EntityContext, References));
        return this;
    }

    protected override IQueryable<TResult> BuildQuery() => _query;
}