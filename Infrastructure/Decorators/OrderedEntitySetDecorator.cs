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
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator to force ordered results after querying
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OrderedEntitySetDecorator<TEntity> : EntitySetDecoratorBase<TEntity> where TEntity : ISimpleEntity, IOrdered {
        /// <summary>
        /// Creates a new instance eof <see cref="OrderedEntitySetDecorator{TEnity}"/>
        /// </summary>
        public OrderedEntitySetDecorator(IEntitySet<TEntity> baseEntitySet) : base(baseEntitySet) { }

        #region Overrides of EntitySetDecoratorBase<TEntity>

        /// <inheritdoc />
        public override async ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys,
	        CancellationToken cancellationToken = default)
        {
	        var asyncResult = BaseEntitySet.GetRangeAsync(keys, cancellationToken);
	        if (!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;
			return asyncResult.Result.OrderBy(e => e.Order);
        }

        /// <inheritdoc />
        public override async ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
	        var asyncResult = BaseEntitySet.GetAllAsync(cancellationToken);
	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;
	        return asyncResult.Result.OrderBy(e => e.Order);
		}

        /// <inheritdoc />
        public override async ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression,
	        CancellationToken cancellationToken = default)
        {
	        var asyncResult = BaseEntitySet.WhereAsync(filterExpression, cancellationToken);
	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;
	        return asyncResult.Result.OrderBy(e => e.Order);
		}

        #endregion

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
	        object InterceptQueryResult(object result)
	        {
		        switch(result)
		        {
			        case IEnumerable<TEntity> entities:
				        return entities.OrderBy(e => e.Order);
		        }

		        return result;
	        }

			return BaseEntitySet.Query()
	                            .InterceptQueryResult(InterceptQueryResult);
        }
    }
}
