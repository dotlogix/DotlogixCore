using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
using DotLogix.Infrastructure.Queries;

namespace DotLogix.Infrastructure.Extensions {
    public static class QueryExtensions {
        public static IQueryable<TResult> Apply<T, TResult>(this IQueryable<T> query, IEntityContext entityContext, IQueryModifier<T, TResult> modifier) {
            return modifier.Apply(entityContext, query);
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, IQueryModifier<T, T> modifier) {
            return modifier != null
                       ? modifier.Apply(entityContext, query)
                       : query;
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, params IQueryModifier<T, T>[] modifiers) {
            return Apply(query, entityContext, modifiers?.AsEnumerable());
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, params IQueryModifier<T>[] modifiers) {
            return Apply(query, entityContext, modifiers?.AsEnumerable());
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, IEnumerable<IQueryModifier<T, T>> modifiers) {
            if(modifiers == null) {
                return query;
            }
            
            return modifiers
                  .SkipNull()
                  .Aggregate(query, (current, modifier) => modifier.Apply(entityContext, current));
        }
    }
}
