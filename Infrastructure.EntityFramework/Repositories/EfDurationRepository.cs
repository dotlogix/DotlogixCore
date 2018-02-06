// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EfDurationEfRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Entities.Options;
using DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using DotLogix.Architecture.Infrastructure.Repositories.Enums;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public abstract class EfDurationRepository<TEntity> : EfRepository<TEntity>, IDurationRepository<TEntity>
        where TEntity : class, ISimpleEntity, IDuration {
        protected EfDurationRepository(IEfEntityContext entityContext) : base(entityContext) { }

        public override void Add(TEntity entity)
        {
            if (entity.FromUtc == default(DateTime))
                entity.FromUtc = DateTime.UtcNow;
            DbSet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            var nowUtc = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                if (entity.FromUtc == default(DateTime))
                    entity.FromUtc = nowUtc;
                DbSet.Add(entity);
            }
        }

        public override void Remove(TEntity entity)
        {
            if (entity.UntilUtc.HasValue == false)
                entity.UntilUtc = DateTime.UtcNow;
        }

        public override void RemoveRange(IEnumerable<TEntity> entities)
        {
            var nowUtc = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                if (entity.UntilUtc.HasValue == false)
                    entity.UntilUtc = nowUtc;
            }
        }

        public async Task<IEnumerable<TEntity>> InRangeAsync(DateTime fromUtc, DateTime untilUtc)
        {
            fromUtc = fromUtc.ToUniversalTime();
            untilUtc = untilUtc.ToUniversalTime();

            return await base.Query().Where(e => e.FromUtc >= fromUtc && e.UntilUtc <= untilUtc).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> StartedInRangeAsync(DateTime fromUtc, DateTime untilUtc)
        {
            fromUtc = fromUtc.ToUniversalTime();
            untilUtc = untilUtc.ToUniversalTime();

            return await base.Query().Where(e => e.FromUtc >= fromUtc && e.FromUtc <= untilUtc).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> EndedInRangeAsync(DateTime fromUtc, DateTime untilUtc)
        {
            fromUtc = fromUtc.ToUniversalTime();
            untilUtc = untilUtc.ToUniversalTime();

            return await base.Query().Where(e => e.UntilUtc >= fromUtc && e.UntilUtc <= untilUtc).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> StartedBeforeAsync(DateTime timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();
            return await Query(DurationFilterModes.Current | DurationFilterModes.Outdated, timeUtc).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> EndedBeforeAsync(DateTime timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();
            return await Query(DurationFilterModes.Outdated, timeUtc).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> StartedAfterAsync(DateTime timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();
            return await Query(DurationFilterModes.InFuture, timeUtc).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> EndedAfterAsync(DateTime timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();
            return await Query(DurationFilterModes.Current | DurationFilterModes.InFuture, timeUtc).ToListAsync();
        }

        #region Queries
        protected override IQueryable<TEntity> Query()
        {
            return Query(DurationFilterModes.Current, DateTime.UtcNow);
        }

        protected IQueryable<TEntity> Query(DurationFilterModes filter, DateTime nowUtc)
        {
            IQueryable<TEntity> entities = DbSet;
            switch (filter)
            {
                case DurationFilterModes.Current:
                    entities = entities.Where(e => e.FromUtc <= nowUtc && e.UntilUtc >= nowUtc);
                    break;
                case DurationFilterModes.InFuture:
                    entities = entities.Where(e => e.FromUtc > nowUtc);
                    break;
                case DurationFilterModes.Outdated:
                    entities = entities.Where(e => e.UntilUtc < nowUtc);
                    break;
                case DurationFilterModes.Current | DurationFilterModes.Outdated:
                    entities = entities.Where(e => e.FromUtc <= nowUtc);
                    break;
                case DurationFilterModes.Current | DurationFilterModes.InFuture:
                    entities = entities.Where(e => e.UntilUtc >= nowUtc);
                    break;
                case DurationFilterModes.InFuture | DurationFilterModes.Outdated:
                    entities = entities.Where(e => e.FromUtc > nowUtc || e.UntilUtc < nowUtc);
                    break;
                case DurationFilterModes.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
            return entities;
        }
        #endregion
    }
}
