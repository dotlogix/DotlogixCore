#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.Infrastructure;
using DotLogix.Infrastructure.Extensions;
using DotLogix.Infrastructure.Queries;
using DotLogix.WebServices.Core.Terms;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class QueryExtensions {
        private static readonly MethodInfo OrderByMethod = ((Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.OrderBy).Method.GetGenericMethodDefinition();
        private static readonly MethodInfo OrderByDescendingMethod = ((Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.OrderByDescending).Method.GetGenericMethodDefinition();
        private static readonly MethodInfo ThenByMethod = ((Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.ThenBy).Method.GetGenericMethodDefinition();
        private static readonly MethodInfo ThenByDescendingMethod = ((Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>)Queryable.ThenByDescending).Method.GetGenericMethodDefinition();

        public static IQueryable<T> WhereRelated<T, TRelated, TKey>(this IQueryable<T> query, IEntityContext entityContext, Expression<Func<T, TKey>> keySelector, Expression<Func<TRelated, TKey>> relatedKeySelector, IQueryModifier<TRelated> relatedModifier = null) where TRelated : class, new() {
            var relatedQueryable = entityContext.GetEntitySet<TRelated>()
                                            .Query()
                                            .Apply(entityContext, relatedModifier);

            var entityExpression = Expression.Parameter(typeof(T), "e");
            var relatedExpression = Expression.Parameter(typeof(TRelated), "e");
            var relatedQueryableExpression = LambdaBuilders.FromQueryable(relatedQueryable);

            var entityProperty = LambdaBuilders.Inline<TKey>(keySelector, LambdaBuilders.From<T>(entityExpression));
            var relatedProperty = LambdaBuilders.Inline<TKey>(relatedKeySelector, LambdaBuilders.From<T>(relatedExpression));

            var equalCondition = entityProperty.Equal(relatedProperty).Lambda<TRelated, bool>(relatedExpression);
            var anyCondition = relatedQueryableExpression.Any(equalCondition).Lambda<T, bool>(entityExpression);
            
            return query.Where(anyCondition);
        }

        public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) {
            return await query.ToListAsync(cancellationToken);
        }
        
        public static async Task<PaginationResult<T>> ToPagedAsync<T>(this IQueryable<T> query, PaginationTerm paginationTerm, CancellationToken cancellationToken = default) {
            List<T> entities;
            int totalCount;

            var page = paginationTerm.Page.GetValueOrDefault(-1);
            var pageSize = page > 0 ? paginationTerm.PageSize : -1;
            var orderBy = paginationTerm.OrderBy;
            if(page > 0) {
                totalCount = await query.CountAsync(cancellationToken);

                query = OrderBy(query, orderBy);
                query = ApplyPage(query, page, pageSize);
                entities = await query.ToListAsync(cancellationToken);
            } else {
                query = OrderBy(query, orderBy);
                entities = await query.ToListAsync(cancellationToken);
                totalCount = entities.Count;
            }

            return new PaginationResult<T> {
                Values = entities,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                OrderBy = orderBy
            };
        }

        private static IQueryable<T> ApplyPage<T>(IQueryable<T> query, int page, int pageSize) {
            return query.Skip(pageSize * page).Take(pageSize);
        }
        
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, ManyTerm<OrderingKey> orderKeys = null) {
            if(orderKeys == null || orderKeys.Count == 0) {
                if(typeof(T).IsAssignableTo<IOrdered>()) {
                    orderKeys = new OrderingKey { Name = nameof(IOrdered.Order), Direction = SortDirection.Ascending };
                } else {
                    throw new ArgumentException("At least one ordering key is required", nameof(orderKeys));
                }
            }
            
            LambdaBuilder builder = LambdaBuilders.FromQueryable(query);
            for(var i = 0; i < orderKeys.Count; i++) {
                var orderingKey = orderKeys[i];
                
                var entity = Expression.Parameter(typeof(T));
                var property = Expression.PropertyOrField(entity, orderingKey.Name);
                var lambda = Expression.Lambda(property, entity);
                
                MethodInfo orderMethod;
                if(i == 0) {
                    orderMethod = orderingKey.Direction == SortDirection.Ascending ? OrderByMethod : OrderByDescendingMethod;
                } else {
                    orderMethod = orderingKey.Direction == SortDirection.Ascending ? ThenByMethod : ThenByDescendingMethod;
                }

                orderMethod = orderMethod.MakeGenericMethod(property.Type);
                builder = builder.CallStatic<IOrderedQueryable<T>>(orderMethod, lambda);
            }
            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(builder.Body);
        }
    }
}
