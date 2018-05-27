using System.Linq;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Query {
    public class EfQueryableQueryFactory : IQueryableQueryFactory
    {
        public static IQueryableQueryFactory Instance { get; } = new EfQueryableQueryFactory();
        private EfQueryableQueryFactory() { }

        public IQuery<T> CreateQuery<T>(IQueryable<T> queryable)
        {
            return new QueryableQuery<T>(queryable, this);
        }

        public IOrderedQuery<T> CreateOrderedQuery<T>(IOrderedQueryable<T> queryable)
        {
            return new OrderedQueryableQuery<T>(queryable, this);
        }

        public IQueryExecutor<T> CreateQueryExecutor<T>(IQueryable<T> queryable)
        {
            return new EfQueryExecutor<T>(queryable);
        }
    }
}