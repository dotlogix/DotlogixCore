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
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator to force ordered results after querying
    /// </summary>
    /// <typeparam name="TEnity"></typeparam>
    public class OrderedEntitySetDecorator<TEnity> : EntitySetDecoratorBase<TEnity> where TEnity : ISimpleEntity, IOrdered {
        /// <summary>
        /// Creates a new instance eof <see cref="OrderedEntitySetDecorator{TEnity}"/>
        /// </summary>
        public OrderedEntitySetDecorator(IEntitySet<TEnity> baseEntitySet) : base(baseEntitySet) { }

        /// <inheritdoc />
        public override IQuery<TEnity> Query() {
            return BaseEntitySet.Query().OrderBy(e => e.Order);
        }

        /// <inheritdoc />
        public override Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetRangeAsync(ids, cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }

        /// <inheritdoc />
        public override Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetRangeAsync(guids, cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }

        /// <inheritdoc />
        public override Task<IEnumerable<TEnity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetAllAsync(cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }

        /// <inheritdoc />
        public override Task<IEnumerable<TEnity>> FilterAllAsync(Expression<Func<TEnity, bool>> filterExpression, CancellationToken cancellationToken) {
            return BaseEntitySet.FilterAllAsync(filterExpression, cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }
    }
}
