using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Entities.Options;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class OrderedEntitySetDecorator<TEnity> : EntitySetDecoratorBase<TEnity> where TEnity : ISimpleEntity, IOrdered {
        public OrderedEntitySetDecorator(IEntitySet<TEnity> baseEntitySet) : base(baseEntitySet) { }
        public override IQuery<TEnity> Query() {
            return BaseEntitySet.Query().OrderBy(e => e.Order);
        }

        public override Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetRangeAsync(ids, cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }

        public override Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetRangeAsync(guids, cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }

        public override Task<IEnumerable<TEnity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetAllAsync(cancellationToken).ConvertResult(r => (IEnumerable<TEnity>)r.OrderBy(e => e.Order));
        }
    }
}