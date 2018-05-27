using System.Linq;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQueryFactory : IQueryableQueryFactory {
        public static IQueryableQueryFactory Instance { get; } = new QueryableQueryFactory();
        private QueryableQueryFactory() { }

        public IQuery<T> CreateQuery<T>(IQueryable<T> queryable) {
            return new QueryableQuery<T>(queryable, this);
        }

        public IOrderedQuery<T> CreateOrderedQuery<T>(IOrderedQueryable<T> queryable) {
            return new OrderedQueryableQuery<T>(queryable, this);
        }

        public IQueryExecutor<T> CreateQueryExecutor<T>(IQueryable<T> queryable)
        {
            return new QueryableQueryExecutor<T>(queryable);
        }
    }
}