﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntityCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// A simple guid and id indexed entity collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityIndex<TEntity> where TEntity : ISimpleEntity {
        /// <summary>
        /// The underlying entity collection for id caching
        /// </summary>
        public EntityCollection<int, TEntity> ById { get; }
        /// <summary>
        /// The underlying entity collection for guid caching
        /// </summary>
        public EntityCollection<Guid, TEntity> ByGuid { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TEntity}"/>
        /// </summary>
        public EntityIndex(EntityCollection<int, TEntity> byId, EntityCollection<Guid, TEntity> byGuid) {
            ById = byId ?? throw new ArgumentNullException(nameof(byId));
            ByGuid = byGuid ?? throw new ArgumentNullException(nameof(byGuid));
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TEntity}"/>
        /// </summary>
        public EntityIndex(IEnumerable<TEntity> entities) {
            if(entities == null)
                throw new ArgumentNullException(nameof(entities));
            entities = entities.AsCollection();
            ById = new EntityCollection<int, TEntity>(e => e.Id, entities);
            ByGuid = new EntityCollection<Guid, TEntity>(e => e.Guid, entities);
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TEntity}"/>
        /// </summary>
        public EntityIndex() {
            ById = new EntityCollection<int, TEntity>(e => e.Id);
            ByGuid = new EntityCollection<Guid, TEntity>(e => e.Guid);
        }
    }


    /// <summary>
    /// A key indexed entity collection
    /// </summary>
    public class EntityCollection<TKey, TEntity> : KeyedCollection<TKey, TEntity> {
        /// <summary>
        /// Creates a new instance of <see cref="EntityCollection{TKey, TEntity}"/>
        /// </summary>
        public EntityCollection(Func<TEntity, TKey> keySelector) : base(keySelector) { }
        /// <summary>
        /// Creates a new instance of <see cref="EntityCollection{TKey, TEntity}"/>
        /// </summary>
        public EntityCollection(Func<TEntity, TKey> keySelector, IEnumerable<TEntity> values) : base(keySelector, values) { }

        /// <summary>
        /// Creates a new instance of <see cref="EntityCollection{TKey, TEntity}"/>
        /// </summary>
        public EntityCollection(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) : base(keySelector, equalityComparer) { }
        /// <summary>
        /// Creates a new instance of <see cref="EntityCollection{TKey, TEntity}"/>
        /// </summary>
        public EntityCollection(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(keySelector, equalityComparer, values) { }


        #region Get
        /// <summary>
        /// Get a range of entities by their key and also return all missing keys
        /// </summary>
        public bool TryGetRange(IEnumerable<TKey> keys, out IEnumerable<TEntity> foundEntities, out IEnumerable<TKey> missingKeys) {
            var found = new List<TEntity>();
            var missing = new List<TKey>();

            foreach(var key in keys) {
                if(InnerDictionary.TryGetValue(key, out var entity) == false)
                    missing.Add(key);
                else if(entity != null)
                    found.Add(entity);
            }

            foundEntities = found;
            missingKeys = missing;
            return missing.Count == 0;
        }
        #endregion

        #region Add
        /// <summary>
        /// Add or update a range of entities by their key and also return all missing keys
        /// </summary>
        public void AddOrUpdateRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                AddOrUpdate(entity);
        }
        #endregion

        #region Remove
        /// <summary>
        /// Tries to remove a range of entities by their key and also return all missing keys
        /// </summary>
        public bool TryRemoveRange(IEnumerable<TEntity> entities) {
            return entities.All(TryRemove);
        }
        #endregion

        #region MarkNonPresent
        /// <summary>
        /// Mark a single entity as non present
        /// </summary>
        public void MarkNonPresent(TKey key) {
            InnerDictionary[key] = default;
        }

        /// <summary>
        /// Mark a range of entities as non present
        /// </summary>
        public void MarkRangeNonPresent(IEnumerable<TKey> keys) {
            foreach(var key in keys)
                InnerDictionary[key] = default;
        }
        #endregion
    }

    /// <summary>
    /// A guid indexed <see cref="EntityCollection{TKey,TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityCollection<TEntity> : EntityCollection<Guid, TEntity> where TEntity : class, ISimpleEntity {
        /// <inheritdoc />
        public EntityCollection() : base(GetKey) { }
        /// <inheritdoc />
        public EntityCollection(IEnumerable<TEntity> values) : base(GetKey, values) { }

        private static Guid GetKey(TEntity entity) {
            return entity.Guid;
        }
    }
}
