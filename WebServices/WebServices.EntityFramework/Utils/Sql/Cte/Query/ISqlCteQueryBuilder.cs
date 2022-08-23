using System;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface ISqlCteQueryBuilder<in TResult> : ISqlCteBuilder<ISqlCteQueryBuilder<TResult>>
{
    ISqlCteQueryBuilder<TResult> UseQuery(Func<ISqlCteContext, IQueryable<TResult>> queryFunc);
}