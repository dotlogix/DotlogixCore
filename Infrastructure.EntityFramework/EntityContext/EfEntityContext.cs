// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Attributes;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    ///     An implementation of the <see cref="IEntityContext" /> interface for entity framework
    /// </summary>
    public class EfEntityContext : IEfEntityContext {
        private static readonly Type GlobalHookType = typeof(void);
        private EfEntityHooks _globalHooks;

        private bool _useHooks;
        private EfEntityHookListeners Listeners { get; set; }
        private Dictionary<Type, object> EntitySetDict { get; } = new Dictionary<Type, object>();

        /// <summary>
        ///     Create a new instance of <see cref="EfEntityContext" />
        /// </summary>
        public EfEntityContext(DbContext dbContext) {
            DbContext = dbContext;
        }

        /// <inheritdoc />
        public DbContext DbContext { get; }

        /// <inheritdoc />
        public EfEntityHooks GlobalHooks => _globalHooks ?? (_globalHooks = GetEntityHooks(typeof(void)));


        /// <inheritdoc />
        public virtual IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, new() {
            return (IEntitySet<TEntity>)EntitySetDict.GetOrAdd(typeof(TEntity), OnCreateEntitySet<TEntity>);
        }

        /// <inheritdoc />
        public virtual async Task CompleteAsync() {
            ICollection<IGrouping<Type, EntityEntry>> groupedEntityEntries = null;
            if(_useHooks && (Listeners.EntityCommit.Count > 0)) {
                var entries = DbContext.ChangeTracker.Entries();
                groupedEntityEntries = entries.GroupBy(e => e.Metadata.ClrType ?? e.Entity?.GetType() ?? typeof(object))
                                              .ToList();
                OnEntitiesCommit(groupedEntityEntries);
            }

            await DbContext.SaveChangesAsync();

            if(_useHooks && (Listeners.EntityCommitted.Count > 0)) {
                if(groupedEntityEntries == null) {
                    var entries = DbContext.ChangeTracker.Entries();
                    groupedEntityEntries = entries.GroupBy(e => e.Metadata.ClrType ?? e.Entity?.GetType() ?? typeof(object))
                                                  .ToList();
                }

                OnEntitiesCommitted(groupedEntityEntries);
            }
        }

        /// <inheritdoc />
        public EfEntityHooks GetEntityHooks<TEntity>() where TEntity : class, new() {
            return GetEntityHooks(typeof(TEntity));
        }

        /// <inheritdoc />
        public virtual EfEntityHooks GetEntityHooks(Type entityType) {
            OnConfigureHooks();
            return new EfEntityHooks(entityType, Listeners);
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual IEntitySet<TEntity> OnCreateEntitySet<TEntity>(Type entityType) where TEntity : class, new() {
            var modifiers = OnCreateModifiers<TEntity>();
            IEntitySet<TEntity> entitySet = new EfEntitySet<TEntity>(this);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach(var modifier in modifiers)
                entitySet = modifier.Invoke(entitySet);
            return entitySet;
        }

        /// <summary>
        ///     Creates entity set modifiers configured for this entity
        /// </summary>
        protected virtual ICollection<Func<IEntitySet<TEntity>, IEntitySet<TEntity>>> OnCreateModifiers<TEntity>() where TEntity : class, new() {
            var entityType = typeof(TEntity);
            var types = new List<Type>();
            types.Add(entityType);
            types.AddRange(entityType.GetTypesAssignableTo());

            var decoratorAttributes = types.SelectMany(t => t.GetCustomAttributes<EntitySetModifierAttribute>());

            return decoratorAttributes
                  .Distinct()
                  .OrderBy(d => d.Priority)
                  .Select(d => d.GetModifierFunc<TEntity>())
                  .ToList();
        }

        /// <summary>
        ///     A callback happening the first time a hook is requested
        /// </summary>
        private void OnConfigureHooks() {
            if(_useHooks)
                return;

            _useHooks = true;

            var entityTracked = new MutableLookup<Type, EventHandler<EntityTrackedEventArgs>>();
            var entityStateChanged = new MutableLookup<Type, EventHandler<EntityStateChangedEventArgs>>();
            var entityCommit = new MutableLookup<Type, EventHandler<EntityCommitEventArgs>>();
            var entityCommitted = new MutableLookup<Type, EventHandler<EntityCommitEventArgs>>();

            Listeners = new EfEntityHookListeners(entityTracked, entityStateChanged, entityCommit, entityCommitted);

            DbContext.ChangeTracker.StateChanged += OnEntityStateChanged;
            DbContext.ChangeTracker.Tracked += OnEntityTracked;
        }

        /// <summary>
        ///     A callback happening if an entity is newly tracked in the db context
        /// </summary>
        protected virtual void OnEntityTracked(object sender, EntityTrackedEventArgs e) {
            var hasEntityHandlers = Listeners.EntityTracked.TryGetValue(e.Entry.Metadata.ClrType, out var entityHandlers) && (entityHandlers.Count > 0);
            var hasGlobalHandlers = Listeners.EntityTracked.TryGetValue(GlobalHookType, out var globalHandlers) && (globalHandlers.Count > 0);

            if(hasGlobalHandlers) {
                foreach(var handler in globalHandlers)
                    handler.Invoke(this, e);
            }

            if(hasEntityHandlers) {
                foreach(var handler in entityHandlers)
                    handler.Invoke(this, e);
            }
        }

        /// <summary>
        ///     A callback happening each time an entity changes it's state. If an entity is newly added to the db context it is
        ///     only track-able with the <see cref="OnEntityTracked" /> method
        /// </summary>
        protected virtual void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e) {
            var hasEntityHandlers = Listeners.EntityStateChanged.TryGetValue(e.Entry.Metadata.ClrType, out var entityHandlers) && (entityHandlers.Count > 0);
            var hasGlobalHandlers = Listeners.EntityStateChanged.TryGetValue(GlobalHookType, out var globalHandlers) && (globalHandlers.Count > 0);

            if(hasGlobalHandlers) {
                foreach(var handler in globalHandlers)
                    handler.Invoke(this, e);
            }

            if(hasEntityHandlers) {
                foreach(var handler in entityHandlers)
                    handler.Invoke(this, e);
            }
        }

        /// <summary>
        ///     A callback happening each time the user commits the store to the database.
        /// </summary>
        /// <param name="entryTypeGroups"></param>
        protected virtual void OnEntitiesCommit(IEnumerable<IGrouping<Type, EntityEntry>> entryTypeGroups) {
            var hasGlobalHandlers = Listeners.EntityCommit.TryGetValue(GlobalHookType, out var globalHandlers) && (globalHandlers.Count > 0);

            foreach(var entryTypeGroup in entryTypeGroups) {
                var hasEntityHandlers = Listeners.EntityCommit.TryGetValue(entryTypeGroup.Key, out var entityHandlers) && (entityHandlers.Count > 0);

                foreach(var entityEntry in entryTypeGroup) {
                    var args = new EntityCommitEventArgs(entityEntry);

                    if(hasGlobalHandlers) {
                        foreach(var handler in globalHandlers)
                            handler.Invoke(this, args);
                    }

                    if(hasEntityHandlers) {
                        foreach(var handler in entityHandlers)
                            handler.Invoke(this, args);
                    }
                }
            }
        }

        /// <summary>
        ///     A callback happening each time the user committed the store to the database.
        /// </summary>
        protected virtual void OnEntitiesCommitted(IEnumerable<IGrouping<Type, EntityEntry>> entryTypeGroups) {
            var hasGlobalHandlers = Listeners.EntityCommitted.TryGetValue(GlobalHookType, out var globalHandlers) && (globalHandlers.Count > 0);

            foreach(var entryTypeGroup in entryTypeGroups) {
                var hasEntityHandlers = Listeners.EntityCommitted.TryGetValue(entryTypeGroup.Key, out var entityHandlers) && (entityHandlers.Count > 0);

                foreach(var entityEntry in entryTypeGroup) {
                    var args = new EntityCommitEventArgs(entityEntry);

                    if(hasGlobalHandlers) {
                        foreach(var handler in globalHandlers)
                            handler.Invoke(this, args);
                    }

                    if(hasEntityHandlers) {
                        foreach(var handler in entityHandlers)
                            handler.Invoke(this, args);
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing)
                DbContext?.Dispose();
        }
    }
}
