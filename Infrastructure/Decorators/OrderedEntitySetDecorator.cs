// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OrderedEntitySetDecorator.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator to force ordered results after querying
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OrderedEntitySetDecorator<TEntity> : EntitySetDecoratorBase<TEntity> where TEntity : class, IOrdered, new() {
        /// <summary>
        /// Creates a new instance eof <see cref="OrderedEntitySetDecorator{TEntity}"/>
        /// </summary>
        public OrderedEntitySetDecorator(IEntitySet<TEntity> baseEntitySet) : base(baseEntitySet) { }

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
            bool InterceptQueryResult(IQueryInterceptionContext queryInterceptionContext) {
                if(queryInterceptionContext.Result.IsUndefined)
                    return true;

		        switch(queryInterceptionContext.Result.Value)
		        {
                    case List<TEntity> entities:
                        entities.Sort(new SelectorComparer<TEntity,int>(e => e.Order));
                        break;
                    case TEntity[] entities:
                        Array.Sort(entities, new SelectorComparer<TEntity, int>(e => e.Order));
                        break;
                    case IEnumerable<TEntity> entities:
                        if(queryInterceptionContext.ResultType == typeof(IEnumerable<TEntity>))
                            queryInterceptionContext.Result = (object)entities.OrderBy(e => e.Order);
                        break;
                }

		        return true;
	        }

			return BaseEntitySet.Query().InterceptQueryResult(InterceptQueryResult);
        }
    }
}
