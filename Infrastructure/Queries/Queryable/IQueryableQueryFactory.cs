using System.Linq;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public interface IQueryableQueryFactory {
        IQuery<T> CreateQuery<T>(IQueryable<T> queryable);
        IOrderedQuery<T> CreateOrderedQuery<T>(IOrderedQueryable<T> queryable);
        IQueryExecutor<T> CreateQueryExecutor<T>(IQueryable<T> queryable);
    }
}