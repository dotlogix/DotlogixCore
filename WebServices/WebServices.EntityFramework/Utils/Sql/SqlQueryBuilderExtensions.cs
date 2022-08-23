using System;
using System.Linq;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public static class SqlQueryBuilderExtensions {
    public static IEntitySet<T> Set<T>(this ISqlQueryContext context) where T : class {
        return context.EntityContext.Set<T>();
    }
    
    public static IQueryable<T> Query<T>(this ISqlQueryContext context) where T : class {
        return context.EntityContext.Query<T>();
    }
    public static IQueryable<T> Query<T>(this ISqlQueryContext context, string sql, params object[] parameters) where T : class{
        return context.EntityContext.Query<T>(sql, parameters);
    }
    public static IQueryable<T> Query<T>(this ISqlQueryContext context, FormattableString sql) where T : class{
        return context.EntityContext.Query<T>(sql);
    }

    public static IQueryable<T> Reference<T>(this ISqlQueryContext context, string referenceName) where T : class {
        if(context.References.TryGetValue(referenceName, out var reference) == false) {
            throw new ArgumentException($"A common table expression with name \"{referenceName}\" is not defined.", nameof(referenceName));
        }
        
        if(reference.Reference is not IQueryable<T> query)
            throw new ArgumentException($"A common table expression of type \"{reference.Type.Name}\" can not be used as query of type \"{typeof(T).GetFriendlyName()}\"");

        return query;
    }

    public static IQueryable<T> UseQuery<T>(this ISqlQueryBuilder builder, string referenceName) where T : class {
        return builder.UseQuery(context => context.Reference<T>(referenceName));
    }
    
    public static ISqlQueryBuilder UseCte<TWith>(this ISqlQueryBuilder builder, Action<ISqlCteQueryBuilder<TWith>> configure) where TWith : class {
        return builder.UseCte(context => {
            var cte = new SqlCteQueryBuilder<TWith>(context.EntityContext, context.References);
            configure(cte);
            return cte.Build();
        });
    }
    
    public static ISqlQueryBuilder UseRecursiveCte<TWith>(this ISqlQueryBuilder builder, Action<IRecursiveSqlCteQueryBuilder<TWith>> configure) where TWith : class {
        return builder.UseCte(context => {
            var cte = new RecursiveSqlCteQueryBuilder<TWith>(context.EntityContext, context.References);
            configure(cte);
            return cte.Build();
        });
    }
    
    public static ISqlQueryBuilder UseHierarchyCte<TWith, TKey>(this ISqlQueryBuilder builder, Action<IHierarchySqlCteBuilder<TWith, TKey>> configure) where TWith : class {
        return builder.UseCte(context => {
            var cte = new HierarchySqlCteBuilder<TWith, TKey>(context.EntityContext, context.References);
            configure(cte);
            return cte.Build();
        });
    }

    public static ISqlQueryBuilder UseCte<TWith>(this ISqlQueryBuilder builder, string referenceName, Func<ISqlQueryContext, IQueryable<TWith>> queryFunc) where TWith : class {
        return builder.UseCte<TWith>(cte => cte
           .UseName(referenceName)
           .UseQuery(queryFunc)
        );
    }

    public static ISqlQueryBuilder UseRecursiveCte<TWith>(this ISqlQueryBuilder builder, string referenceName, Func<IRecursiveSqlCteContext<TWith>, IQueryable<TWith>> queryFunc) where TWith : class {
        return builder.UseRecursiveCte<TWith>(cte => cte
           .UseName(referenceName)
           .UseQuery(queryFunc)
        );
    }
}