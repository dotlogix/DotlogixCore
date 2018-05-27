using System;
using System.Linq.Expressions;

namespace DotLogix.Architecture.Infrastructure.Queries {
    public interface IOrderedQuery<T> : IQuery<T>
    {
        IOrderedQuery<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IOrderedQuery<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}