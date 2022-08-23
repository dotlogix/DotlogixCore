using System;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface ISqlQueryBuilder {
    ISqlQueryBuilder UseCte(Func<ISqlQueryContext, ISqlCte> cteFunc);
    IQueryable<TResult> UseQuery<TResult>(Func<ISqlQueryContext, IQueryable<TResult>> queryFunc) where TResult : class;
}