using System;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface IRecursiveSqlCteQueryBuilder<TResult> : ISqlCteBuilder<IRecursiveSqlCteQueryBuilder<TResult>>
{
    IRecursiveSqlCteQueryBuilder<TResult> UseQuery(Func<IRecursiveSqlCteContext<TResult>, IQueryable<TResult>> queryFunc);
}