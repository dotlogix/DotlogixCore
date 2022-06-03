#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.Core.Expressions;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Database;
#endregion

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class QueryExtensions {
        public static IQueryable<TResult> Apply<T, TResult>(this IQueryable<T> query, IEntityContext entityContext, IEntityQuery<T, TResult> modifier) {
            return modifier.Apply(entityContext, query);
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, IEntityQuery<T, T> modifier) {
            return modifier.Apply(entityContext, query);
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, params IEntityQuery<T, T>[] modifiers) {
            return Apply(query, entityContext, modifiers?.AsEnumerable());
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, params IEntityQuery<T>[] modifiers) {
            return Apply(query, entityContext, modifiers?.AsEnumerable());
        }
        
        public static IQueryable<T> Apply<T>(this IQueryable<T> query, IEntityContext entityContext, IEnumerable<IEntityQuery<T, T>> modifiers) {
            return modifiers.Aggregate(query, (q, m) => m.Apply(entityContext, q));
        }
        
        public static IQueryable<T> WhereRelated<T, TRelated, TKey>(
            this IQueryable<T> query,
            IEntityContext entityContext, 
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TRelated, TKey>> relatedKeySelector, 
            IEntityQuery<TRelated> relatedModifier = null
        ) where TRelated : class, new() {
            var relatedQuery = entityContext.GetEntitySet<TRelated>().Query();
            if(relatedModifier is not null)
                relatedQuery = relatedModifier.Apply(entityContext, relatedQuery);
            return WhereRelated(query, keySelector, relatedQuery, relatedKeySelector);
        }

        public static IQueryable<T> WhereRelated<T, TMapping, TRelated, TKey, TRelatedKey>(
            this IQueryable<T> query,
            IEntityContext entityContext,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TMapping, TKey>> mappingKeySelector,
            Expression<Func<TMapping, TRelatedKey>> mappingRelatedKeySelector,
            Expression<Func<TRelated, TRelatedKey>> relatedKeySelector,
            IEntityQuery<TRelated> relatedModifier = null
        ) where TRelated : class, new() where TMapping : class, new() {
            
            var relatedQuery = entityContext.GetEntitySet<TRelated>().Query();
            if(relatedModifier is not null)
                relatedQuery = relatedModifier.Apply(entityContext, relatedQuery);

            var mappingQuery = entityContext.GetEntitySet<TMapping>().Query();
            mappingQuery = mappingQuery.WhereRelated(mappingRelatedKeySelector, relatedQuery, relatedKeySelector);

            query = query.WhereRelated(keySelector, mappingQuery, mappingKeySelector);
            return query;
        }

        public static IQueryable<T> WhereRelated<T, TRelated, TKey>(
            this IQueryable<T> query, 
            Expression<Func<T, TKey>> keySelector,
            IQueryable<TRelated> relatedQuery,
            Expression<Func<TRelated, TKey>> relatedKeySelector
        ) where TRelated : class, new() {
            var entityExpression = Expression.Parameter(typeof(T), "e");
            var relatedExpression = Expression.Parameter(typeof(TRelated), "e");
            var relatedQueryableExpression = Lambdas.FromQueryable(relatedQuery);

            var entityProperty = Lambdas.Invoke(keySelector, Lambdas.From<T>(entityExpression));
            var relatedProperty = Lambdas.Invoke(relatedKeySelector, Lambdas.From<TRelated>(relatedExpression));

            var equalCondition = entityProperty.IsEqualTo(relatedProperty).ToLambda<TRelated, bool>(relatedExpression);
            var anyCondition = relatedQueryableExpression.Any(equalCondition).ToLambda<T, bool>(entityExpression);

            return query.Where(anyCondition);
        }
    }
}
