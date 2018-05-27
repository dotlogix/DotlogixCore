using System;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class OrderedQueryableQuery<TSource> : QueryableQuery<TSource>, IOrderedQuery<TSource>
    {
        private IOrderedQueryable<TSource> _innerQueryable;
        public OrderedQueryableQuery(IOrderedQueryable<TSource> innerQueryable, IQueryableQueryFactory factory) : base(innerQueryable, factory)
        {
            _innerQueryable = innerQueryable;
        }

        public IOrderedQuery<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            _innerQueryable = _innerQueryable.ThenBy(keySelector);
            return this;
        }

        public IOrderedQuery<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            _innerQueryable = _innerQueryable.ThenByDescending(keySelector);
            return this;
        }
    }
}